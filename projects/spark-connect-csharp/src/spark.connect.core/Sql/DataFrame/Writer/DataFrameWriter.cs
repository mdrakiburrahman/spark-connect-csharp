// -----------------------------------------------------------------------------
// <summary>
// A data frame writer.
// </summary>
//
// <copyright file="DataFrameWriter.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.Session;

namespace Spark.Connect.Core.Sql.DataFrame.Writer
{
    /// <summary>
    /// DataFrameWriter supports writing data frame to storage.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DataFrameWriter"/> class.
    /// </remarks>
    /// <param name="sparkSession">The Spark Session.</param>
    /// <param name="relation">An explicit relationship type.</param>
    /// <param name="saveMode">The save mode.</param>
    /// <param name="formatSource">The format source.</param>
    public class DataFrameWriter(
        ISparkSession sparkSession,
        Relation relation,
        string saveMode = "",
        string formatSource = ""
        ) : IDataFrameWriter
    {
        private readonly ISparkSession sparkSession = sparkSession;
        private readonly Relation relation = relation;
        private string saveMode = saveMode;
        private string formatSource = formatSource;

        /// <inheritdoc/>
        public IDataFrameWriter Mode(string saveMode)
        {
            this.saveMode = saveMode;
            return this;
        }

        /// <inheritdoc/>
        public IDataFrameWriter Format(string source)
        {
            this.formatSource = source;
            return this;
        }

        /// <inheritdoc/>
        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the save mode based on the specified mode string.
        /// </summary>
        /// <param name="mode">The mode string.</param>
        /// <returns>The save mode.</returns>
        private static WriteOperation.Types.SaveMode GetSaveMode(string mode)
        {
            return mode switch
            {
                "" => WriteOperation.Types.SaveMode.Unspecified,
                "Append" => WriteOperation.Types.SaveMode.Append,
                "Overwrite" => WriteOperation.Types.SaveMode.Overwrite,
                "ErrorIfExists" => WriteOperation.Types.SaveMode.ErrorIfExists,
                "Ignore" => WriteOperation.Types.SaveMode.Ignore,
                _ => throw new ArgumentException($"Unsupported save mode: {mode}"),
            };
        }
    }
}
