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

using Spark.Connect.Core.Sql.DataFrame.Columns;
using Spark.Connect.Core.Sql.DataFrame.Rows;
using Spark.Connect.Core.Sql.DataFrame.Types;
using Spark.Connect.Core.Sql.DataFrame.Writer;
using Spark.Connect.Core.Sql.Session;

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
            // TODO: Implement
            throw new System.NotImplementedException();
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
    }
}
