// -----------------------------------------------------------------------------
// <summary>
// A Spark Session Builder.
// </summary>
//
// <copyright file="SparkSessionBuilder.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Grpc.Core;

using Spark.Connect.Core.Channel.Builder;

namespace Spark.Connect.Core.Sql.Session.Builder
{
    /// <summary>
    /// A Spark Session Builder.
    /// </summary>
    public class SparkSessionBuilder : ISparkSessionBuilder
    {
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkSessionBuilder"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SparkSessionBuilder(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Builds the Spark session.
        /// </summary>
        /// <returns>The Spark session.</returns>
        public ISparkSession Build()
        {
            var cb = new ChannelBuilder(this.connectionString);
            try
            {
                var conn = cb.Build();

                var meta = new Metadata();
                foreach (var (k, v) in cb.Headers)
                {
                    meta.Add(k, v);
                }

                var client = new SparkConnectService.SparkConnectServiceClient(conn);
                return new SparkSession(Guid.NewGuid().ToString(), client, meta);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to connect to remote {this.connectionString}: {ex.Message}",
                    ex
                );
            }
        }
    }
}
