// -----------------------------------------------------------------------------
// <summary>
// A contract for reading data frames.
// </summary>
//
// <copyright file="IDataFrameReader.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Reader
{
    /// <summary>
    /// Supports reading data from storage and returning a data frame.
    /// </summary>
    public interface IDataFrameReader
    {
        /// <summary>
        /// Specifies data format (data source type) for the underlying data, e.g. parquet.
        /// </summary>
        /// <param name="source">The data format for the underlying data.</param>
        /// <returns>A data frame reader.</returns>
        IDataFrameReader Format(string source);

        /// <summary>
        /// Reads the underlying data and returns a data frame.
        /// </summary>
        /// <param name="path">The path to read the data from.</param>
        /// <returns>The data frame.</returns>
        IDataFrame Load(string path);
    }
}
