// -----------------------------------------------------------------------------
// <summary>
// A contract for a Spark Session.
// </summary>
//
// <copyright file="ISparkSession.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.DataFrame;
using Spark.Connect.Core.Sql.DataFrame.Exceptions;
using Spark.Connect.Core.Sql.DataFrame.Reader;

namespace Spark.Connect.Core.Sql.Session
{
    /// <summary>
    /// Represents a Spark session.
    /// </summary>
    public interface ISparkSession
    {
        /// <summary>
        /// Returns a data frame reader.
        /// </summary>
        /// <returns>A data frame reader.</returns>
        IDataFrameReader Read();

        /// <summary>
        /// Executes a SQL query and returns the result as a data frame.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <returns>The result as a data frame.</returns>
        IDataFrame Sql(string query);

        /// <summary>
        /// Stops the Spark session.
        /// </summary>
        /// <exception cref="SparkSessionException">Thrown when an error occurs while stopping the exception.</exception>
        void Stop();
    }
}
