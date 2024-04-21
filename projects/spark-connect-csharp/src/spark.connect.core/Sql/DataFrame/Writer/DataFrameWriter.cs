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

using Spark.Connect.Core.Channel.Extension;
using Spark.Connect.Core.Sql.Session;

using static Spark.Connect.WriteOperation.Types;

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
        SparkSession sparkSession,
        Relation relation,
        string saveMode = "",
        string formatSource = ""
        ) : IDataFrameWriter
    {
        private readonly SparkSession sparkSession = sparkSession;
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
            SaveMode saveMode;
            try
            {
                saveMode = GetSaveMode(this.saveMode);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting save mode", ex);
            }

            string? source = null;
            if (!string.IsNullOrEmpty(this.formatSource))
            {
                source = this.formatSource;
            }

            var plan = new Plan
            {
                Command = new Command
                {
                    WriteOperation = new WriteOperation
                    {
                        Input = this.relation,
                        Mode = saveMode,
                        Source = source,
                        Path = path,
                    },
                },
            };

            try
            {
                this.sparkSession.ExecutePlan(plan).ExecuteRpc();
            }
            catch (Exception ex)
            {
                throw new Exception("Error consuming execute plan client", ex);
            }
        }

        /// <summary>
        /// Gets the save mode based on the specified mode string.
        /// </summary>
        /// <param name="mode">The mode string.</param>
        /// <returns>The save mode.</returns>
        private static WriteOperation.Types.SaveMode GetSaveMode(string mode)
        {
            return mode.ToLower() switch
            {
                "" => WriteOperation.Types.SaveMode.Unspecified,
                "append" => WriteOperation.Types.SaveMode.Append,
                "overwrite" => WriteOperation.Types.SaveMode.Overwrite,
                "errorifexists" => WriteOperation.Types.SaveMode.ErrorIfExists,
                "ignore" => WriteOperation.Types.SaveMode.Ignore,
                _ => throw new ArgumentException($"Unsupported save mode: {mode}"),
            };
        }
    }
}
