// -----------------------------------------------------------------------------
// <summary>
// A Data Frame reader.
// </summary>
//
// <copyright file="DataFrameReader.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.Session;

using static Spark.Connect.Read.Types;

namespace Spark.Connect.Core.Sql.DataFrame.Reader
{
    /// <summary>
    /// DataFrameReader supports reading data frames.
    /// </summary>
    public class DataFrameReader : IDataFrameReader
    {
        private readonly SparkSession sparkSession;
        private string formatSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFrameReader"/> class.
        /// </summary>
        /// <param name="sparkSession">The Spark Session.</param>
        /// <param name="formatSource">The Format Source.</param>
        public DataFrameReader(SparkSession sparkSession, string formatSource = "")
        {
            this.sparkSession = sparkSession;
            this.formatSource = formatSource;
        }

        /// <inheritdoc/>
        public IDataFrameReader Format(string source)
        {
            this.formatSource = source;
            return this;
        }

        /// <inheritdoc/>
        public IDataFrame Load(string path)
        {
            var dataSource = new DataSource
            {
                Format = string.IsNullOrEmpty(this.formatSource) ? null : this.formatSource,
            };
            dataSource.Paths.Add(path);

            var relation = new Relation { Read = new Read { DataSource = dataSource, }, };

            var df = new DataFrame(this.sparkSession, relation);
            return df;
        }
    }
}
