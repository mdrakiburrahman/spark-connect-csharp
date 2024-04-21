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

using Spark.Connect.Core.Sql.DataFrame.Columns;
using Spark.Connect.Core.Sql.DataFrame.Rows;
using Spark.Connect.Core.Sql.DataFrame.Types;
using Spark.Connect.Core.Sql.DataFrame.Writer;
using Spark.Connect.Core.Sql.Session;

using StructType = Spark.Connect.Core.Sql.DataFrame.Types.StructType;

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
            while (true)
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

            throw new Exception("Did not get arrow batch in response");
        }

        /// <inheritdoc/>
        public StructType Schema()
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IRow[] Collect()
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IDataFrameWriter Write()
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void CreateTempView(string viewName, bool replace, bool global)
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IDataFrame Repartition(int numPartitions, string[] columns)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IDataFrame RepartitionByRange(int numPartitions, RangePartitionColumn[] columns)
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }

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

        private List<Row> ReadArrowBatchData(byte[] data, StructType? schema)
        {
            // TODO: Implement
            throw new NotImplementedException();
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
    }
}
