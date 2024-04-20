// -----------------------------------------------------------------------------
// <summary>
// A Spark Session.
// </summary>
//
// <copyright file="SparkSession.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Grpc.Core;

using Spark.Connect.Core.Sql.DataFrame;
using Spark.Connect.Core.Sql.DataFrame.Reader;

namespace Spark.Connect.Core.Sql.Session
{
    /// <summary>
    /// A Spark Session.
    /// </summary>
    public class SparkSession : ISparkSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SparkSession"/> class.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="client">The Spark Connect service client.</param>
        /// <param name="metadata">The metadata.</param>
        public SparkSession(
            string sessionId,
            SparkConnectService.SparkConnectServiceClient client,
            Metadata metadata
        )
        {
            this.SessionId = sessionId;
            this.Client = client;
            this.Metadata = metadata;
        }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the Spark Connect service client.
        /// </summary>
        public SparkConnectService.SparkConnectServiceClient Client { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Metadata Metadata { get; set; }

        /// <inheritdoc/>
        public IDataFrameReader Read()
        {
            return new DataFrameReader(this);
        }

        /// <inheritdoc/>
        public IDataFrame Sql(string query)
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            // Not implemented in spark-connect-go either.
            return;
        }
    }
}
