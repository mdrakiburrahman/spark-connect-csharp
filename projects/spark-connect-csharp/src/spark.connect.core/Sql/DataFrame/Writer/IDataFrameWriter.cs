// -----------------------------------------------------------------------------
// <summary>
// A contract for writing data frames.
// </summary>
//
// <copyright file="IDataFrameWriter.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Writer
{
    /// <summary>
    /// A contract for writing data frames.
    /// </summary>
    public interface IDataFrameWriter
    {
        /// <summary>
        /// Specifies the saving mode for the data, e.g. Append, Overwrite, ErrorIfExists.
        /// </summary>
        /// <param name="saveMode">The saving mode for the data.</param>
        /// <returns>The data frame writer.</returns>
        IDataFrameWriter Mode(string saveMode);

        /// <summary>
        /// Specifies the data format (data source type) for the underlying data, e.g. parquet.
        /// </summary>
        /// <param name="source">The data format for the underlying data.</param>
        /// <returns>The data frame writer.</returns>
        IDataFrameWriter Format(string source);

        /// <summary>
        /// Writes the data frame to the given path.
        /// </summary>
        /// <param name="path">The path to write the data frame to.</param>
        void Save(string path);
    }
}
