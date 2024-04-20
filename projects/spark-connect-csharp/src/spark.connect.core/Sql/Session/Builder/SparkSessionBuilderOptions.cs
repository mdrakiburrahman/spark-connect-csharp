// -----------------------------------------------------------------------------
// <summary>
// An internal class for Spark Session Builder Options.
// </summary>
//
// <copyright file="SparkSessionBuilderOptions.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Grpc.Core;

namespace Spark.Connect.Core.Sql.Session.Builder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SparkSessionBuilderOptions"/> class with the specified values.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    /// <param name="client">The Spark Connect service client.</param>
    /// <param name="metadata">The metadata.</param>
    internal class SparkSessionBuilderOptions(
        string sessionId,
        SparkConnectService.SparkConnectServiceClient client,
        Metadata metadata
        )
    {
        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        public string SessionId { get; set; } = sessionId;

        /// <summary>
        /// Gets or sets the Spark Connect service client.
        /// </summary>
        public SparkConnectService.SparkConnectServiceClient Client { get; set; } = client;

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Metadata Metadata { get; set; } = metadata;
    }
}
