// -----------------------------------------------------------------------------
// <summary>
// A Data Frame.
// </summary>
//
// <copyright file="DataFrame.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Apache.Arrow;
using Apache.Arrow.Ipc;
using Apache.Arrow.Types;

using Grpc.Core;

using Spark.Connect.Core.Channel.Extension;
using Spark.Connect.Core.Sql.DataFrame.Columns;
using Spark.Connect.Core.Sql.DataFrame.Exceptions;
using Spark.Connect.Core.Sql.DataFrame.Rows;
using Spark.Connect.Core.Sql.DataFrame.Types;
using Spark.Connect.Core.Sql.DataFrame.Writer;
using Spark.Connect.Core.Sql.Session;

using static Spark.Connect.Expression.Types;

namespace Spark.Connect.Core.Sql.DataFrame
{
    /// <summary>
    /// A DataFrame.
    /// </summary>
    public class DataFrame : IDataFrame
    {
        private readonly SparkSession sparkSession;
        private readonly Relation relation;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFrame"/> class.
        /// </summary>
        /// <param name="sparkSession">The Spark Session.</param>
        /// <param name="relation">An explicit relationship type.</param>
        public DataFrame(SparkSession sparkSession, Relation relation)
        {
            this.sparkSession = sparkSession;
            this.relation = relation;
        }

        #region IDataFrame Implementation

        /// <inheritdoc/>
        public void Show(int numRows, bool truncate)
        {
            int truncateValue = truncate ? 20 : 0;
            bool vertical = false;

            var plan = new Plan
            {
                Root = new Relation
                {
                    Common = new RelationCommon { PlanId = PlanIdGenerator.NewPlanId(), },
                    ShowString = new ShowString
                    {
                        Input = this.relation,
                        NumRows = numRows,
                        Truncate = truncateValue,
                        Vertical = vertical,
                    },
                },
            };

            var responseClient = this.sparkSession.ExecutePlan(plan);
            while (responseClient.ResponseStream.MoveNext(CancellationToken.None).Result)
            {
                var response = responseClient.ResponseStream.Current;
                var arrowBatch = response.ArrowBatch;
                if (arrowBatch == null)
                {
                    continue;
                }

                this.ShowArrowBatch(arrowBatch);
                return;
            }

            throw new NoDataReceivedException("Arrow Batch");
        }

        /// <inheritdoc/>
        public Spark.Connect.Core.Sql.DataFrame.Types.StructType Schema()
        {
            try
            {
                var response = this.sparkSession.AnalyzePlan(this.CreatePlan());
                var responseSchema = response.Schema.Schema_;
                var result = this.ConvertProtoDataTypeToStructType(responseSchema);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get schema", ex);
            }
        }

        /// <inheritdoc/>
        public IRow[] Collect()
        {
            var responseClient = this.sparkSession.ExecutePlan(this.CreatePlan());
            Spark.Connect.Core.Sql.DataFrame.Types.StructType? schema = default;
            var allRows = new List<IRow>();

            while (responseClient.ResponseStream.MoveNext().Result)
            {
                var response = responseClient.ResponseStream.Current;
                if (response == null)
                {
                    return allRows.ToArray();
                }

                var dataType = response.Schema;
                if (dataType != null)
                {
                    schema = this.ConvertProtoDataTypeToStructType(dataType);
                    continue;
                }

                var arrowBatch = response.ArrowBatch;
                if (arrowBatch == null)
                {
                    continue;
                }

                var rowBatch = this.ReadArrowBatchData(arrowBatch.Data.ToByteArray(), schema);
                if (rowBatch == null)
                {
                    throw new Exception("Failed to read arrow batch data");
                }

                allRows.AddRange(rowBatch);
            }

            return allRows.ToArray();
        }

        /// <inheritdoc/>
        public IDataFrameWriter Write()
        {
            return new DataFrameWriter(this.sparkSession, this.relation);
        }

        /// <inheritdoc/>
        public void CreateTempView(string viewName, bool replace, bool global)
        {
            try
            {
                var plan = new Plan
                {
                    Command = new Command
                    {
                        CreateDataframeView = new CreateDataFrameViewCommand
                        {
                            Input = this.relation,
                            Name = viewName,
                            Replace = replace,
                            IsGlobal = global,
                        },
                    },
                };

                this.sparkSession.ExecutePlan(plan).ExecuteRpc();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create temp view {viewName}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public IDataFrame Repartition(int numPartitions, string[] columns)
        {
            List<Expression> partitionExpressions = new List<Expression>();
            if (columns != null)
            {
                partitionExpressions = new List<Expression>(columns.Length);
                foreach (var c in columns)
                {
                    var expr = new Expression
                    {
                        UnresolvedAttribute = new UnresolvedAttribute { UnparsedIdentifier = c, },
                    };
                    partitionExpressions.Add(expr);
                }
            }

            return this.RepartitionByExpressions(numPartitions, partitionExpressions);
        }

        /// <inheritdoc/>
        public IDataFrame RepartitionByRange(int numPartitions, RangePartitionColumn[] columns)
        {
            List<Expression> partitionExpressions = new List<Expression>();
            if (columns != null)
            {
                partitionExpressions = new List<Expression>(columns.Length);
                foreach (var c in columns)
                {
                    var expr = new Expression
                    {
                        UnresolvedAttribute = new UnresolvedAttribute { UnparsedIdentifier = c.ToString(), },
                    };

                    var direction = Expression.Types.SortOrder.Types.SortDirection.Ascending;
                    if (c.Descending)
                    {
                        direction = Expression.Types.SortOrder.Types.SortDirection.Descending;
                    }

                    var sortExpr = new Expression
                    {
                        SortOrder = new SortOrder { Child = expr, Direction = direction, },
                    };

                    partitionExpressions.Add(sortExpr);
                }
            }

            return this.RepartitionByExpressions(numPartitions, partitionExpressions);
        }

        #endregion IDataFrame Implementation

        #region Arrow Data Access Private Methods

        /// <summary>
        /// Shows the data in an Arrow batch.
        /// </summary>
        /// <param name="arrowBatch">The Arrow batch.</param>
        private void ShowArrowBatch(ExecutePlanResponse.Types.ArrowBatch arrowBatch)
        {
            this.ShowArrowBatchData(arrowBatch.Data.ToByteArray());
        }

        /// <summary>
        /// Shows the data in an Arrow batch.
        /// </summary>
        /// <param name="data">The byte array representing the Arrow batch.</param>
        private void ShowArrowBatchData(byte[] data)
        {
            var rows = this.ReadArrowBatchData(data, null);
            foreach (var row in rows)
            {
                var values = row.Values();
                if (values == null)
                {
                    throw new Exception("Failed to get values in the row");
                }

                Console.WriteLine(string.Join(", ", values));
            }
        }

        /// <summary>
        /// Reads the data from an Arrow batch and converts it into a list of rows.
        /// </summary>
        /// <param name="data">The byte array representing the Arrow batch.</param>
        /// <param name="schema">The schema of the data.</param>
        /// <returns>A list of rows.</returns>
        private List<Row> ReadArrowBatchData(
            byte[] data,
            Spark.Connect.Core.Sql.DataFrame.Types.StructType? schema
        )
        {
            var reader = new ArrowStreamReader(new MemoryStream(data));
            var rows = new List<Row>();
            RecordBatch recordBatch;

            while ((recordBatch = reader.ReadNextRecordBatch()) != null)
            {
                var values = this.ReadArrowRecord(recordBatch);

                foreach (var v in values)
                {
                    var row = new Row(v, schema);
                    rows.Add(row);
                }
            }

            return rows;
        }

        /// <summary>
        /// Reads the value of a column from an Arrow record batch.
        /// </summary>
        /// <param name="record">The Arrow record batch.</param>
        /// <returns>The array of column values.</returns>
        private object[][] ReadArrowRecord(RecordBatch record)
        {
            int numRows = record.Length;
            int numColumns = record.Schema.FieldsList.Count;

            var values = new object[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                values[i] = new object[numColumns];
            }

            for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                this.ReadArrowRecordColumn(record, columnIndex, values);
            }

            return values;
        }

        /// <summary>
        /// Reads the value of a column from an Arrow record batch.
        /// </summary>
        /// <param name="record">The Arrow record batch.</param>
        /// <param name="columnIndex">The index of the column.</param>
        /// <param name="values">The array to store the column values.</param>
        private void ReadArrowRecordColumn(RecordBatch record, int columnIndex, object[][] values)
        {
            int numRows = record.Length;
            var column = record.Column(columnIndex);

            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                switch (column.Data.DataType.TypeId)
                {
                    case ArrowTypeId.Boolean:
#pragma warning disable CS8601, CS8602 // Possible null reference assignment.
                        values[rowIndex][columnIndex] = (column as BooleanArray).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.Int8:
                        values[rowIndex][columnIndex] = (column as Int8Array).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.Int16:
                        values[rowIndex][columnIndex] = (column as Int16Array).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.Int32:
                        values[rowIndex][columnIndex] = (column as Int32Array).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.Int64:
                        values[rowIndex][columnIndex] = (column as Int64Array).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.Float:
                        values[rowIndex][columnIndex] = (column as FloatArray).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.Double:
                        values[rowIndex][columnIndex] = (column as DoubleArray).GetValue(rowIndex);
                        break;
                    case ArrowTypeId.String:
                        values[rowIndex][columnIndex] = (column as StringArray).GetString(rowIndex);
                        break;
                    case ArrowTypeId.Binary:
                        values[rowIndex][columnIndex] = (column as BinaryArray)
                            .GetBytes(rowIndex)
                            .ToArray();
                        break;
                    case ArrowTypeId.Timestamp:
                        values[rowIndex][columnIndex] = (column as TimestampArray).GetTimestamp(
                            rowIndex
                        );
                        break;
                    case ArrowTypeId.Date64:
                        values[rowIndex][columnIndex] = (column as Date64Array).GetDateTimeOffset(
                            rowIndex
                        );
                        break;
#pragma warning restore CS8601, CS8602 // Possible null reference assignment.
                    default:
                        throw new Exception(
                            $"Unsupported arrow data type {column.Data.DataType.TypeId} in column {columnIndex}"
                        );
                }
            }
        }

        #endregion Arrow Data Access Private Methods

        #region Protobuf to Spark Data Type Conversion Private Methods

        /// <summary>
        /// Converts the given Proto struct type to a corresponding StructType in the DataFrame.
        /// </summary>
        /// <param name="input">The Proto struct types to convert.</param>
        /// <returns>The corresponding StructType in the DataFrame.</returns>
        private Spark.Connect.Core.Sql.DataFrame.Types.StructType ConvertProtoDataTypeToStructType(
            Spark.Connect.DataType input
        )
        {
            var dataTypeStruct = input.Struct;
            if (dataTypeStruct == null)
            {
                throw new InvalidOperationException("dataType.Struct is null");
            }

            return new Spark.Connect.Core.Sql.DataFrame.Types.StructType(
                string.Empty,
                this.ConvertProtoStructFields(dataTypeStruct.Fields.ToList())
            );
        }

        /// <summary>
        /// Converts the given Proto struct fields to a corresponding list of StructFields in the DataFrame.
        /// </summary>
        /// <param name="input">The Proto struct fields to convert.</param>
        /// <returns>The corresponding list of StructFields in the DataFrame.</returns>
        private List<Spark.Connect.Core.Sql.DataFrame.Fields.StructField> ConvertProtoStructFields(
            List<Spark.Connect.DataType.Types.StructField> input
        )
        {
            var result = new List<Spark.Connect.Core.Sql.DataFrame.Fields.StructField>(input.Count);
            foreach (var f in input)
            {
                result.Add(this.ConvertProtoStructField(f));
            }

            return result;
        }

        /// <summary>
        /// Converts the given Proto struct field to a corresponding StructField in the DataFrame.
        /// </summary>
        /// <param name="field">The Proto struct field to convert.</param>
        /// <returns>The corresponding StructField in the DataFrame.</returns>
        private Spark.Connect.Core.Sql.DataFrame.Fields.StructField ConvertProtoStructField(
            Spark.Connect.DataType.Types.StructField field
        )
        {
            return new Spark.Connect.Core.Sql.DataFrame.Fields.StructField(
                field.Name,
                this.ConvertProtoDataTypeToDataType(field.DataType)
            );
        }

        /// <summary>
        /// Converts the given Proto data type to a corresponding data type in the DataFrame.
        /// </summary>
        /// <param name="input">The Proto data type to convert.</param>
        /// <returns>The corresponding data type in the DataFrame.</returns>
        private IDataType ConvertProtoDataTypeToDataType(Spark.Connect.DataType input)
        {
            switch (input.KindCase)
            {
                case Spark.Connect.DataType.KindOneofCase.Boolean:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.BooleanType();
                case Spark.Connect.DataType.KindOneofCase.Byte:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.ByteType();
                case Spark.Connect.DataType.KindOneofCase.Short:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.ShortType();
                case Spark.Connect.DataType.KindOneofCase.Integer:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.IntegerType();
                case Spark.Connect.DataType.KindOneofCase.Long:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.LongType();
                case Spark.Connect.DataType.KindOneofCase.Float:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.FloatType();
                case Spark.Connect.DataType.KindOneofCase.Double:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.DoubleType();
                case Spark.Connect.DataType.KindOneofCase.Decimal:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.DecimalType();
                case Spark.Connect.DataType.KindOneofCase.String:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.StringType();
                case Spark.Connect.DataType.KindOneofCase.Binary:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.BinaryType();
                case Spark.Connect.DataType.KindOneofCase.Timestamp:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.TimestampType();
                case Spark.Connect.DataType.KindOneofCase.TimestampNtz:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.TimestampNTZType();
                case Spark.Connect.DataType.KindOneofCase.Date:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.DateType();
                default:
                    return new Spark.Connect.Core.Sql.DataFrame.Types.UnsupportedType(
                        input.KindCase.ToString()
                    );
            }
        }

        #endregion Protobuf to Spark Data Type Conversion Private Methods

        #region Plan Creation Private Methods

        /// <summary>
        /// Creates a read plan for the DataFrame.
        /// </summary>
        /// <returns>The created read plan.</returns>
        private Plan CreatePlan()
        {
            return new Plan
            {
                Root = this.relation,
            };
        }

        /// <summary>
        /// Repartitions the DataFrame by the specified number of partitions and partition expressions.
        /// </summary>
        /// <param name="numPartitions">The number of partitions to repartition the DataFrame into.</param>
        /// <param name="partitionExpressions">The list of partition expressions.</param>
        /// <returns>The repartitioned DataFrame.</returns>
        private IDataFrame RepartitionByExpressions(
            int numPartitions,
            List<Spark.Connect.Expression> partitionExpressions
        )
        {
            int numPartitionsPointerValue = 1;
            if (numPartitions != 0)
            {
                numPartitionsPointerValue = numPartitions;
            }

            var newRelation = new Relation
            {
                Common = new RelationCommon { PlanId = PlanIdGenerator.NewPlanId(), },
                RepartitionByExpression = new RepartitionByExpression
                {
                    Input = this.relation,
                    NumPartitions = numPartitionsPointerValue,
                    PartitionExprs = { partitionExpressions },
                },
            };

            return new DataFrame(this.sparkSession, newRelation);
        }

        #endregion
    }
}
