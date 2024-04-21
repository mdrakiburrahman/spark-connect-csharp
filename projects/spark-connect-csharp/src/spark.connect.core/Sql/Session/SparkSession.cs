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
using Spark.Connect.Core.Sql.DataFrame.Exceptions;
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
            var plan = new Plan
            {
                Command = new Command { SqlCommand = new SqlCommand { Sql = query, }, },
            };

            AsyncServerStreamingCall<ExecutePlanResponse> responseClient;
            try
            {
                responseClient = this.ExecutePlan(plan);
            }
            catch (Exception e)
            {
                throw new QueryExecutionException($"{query} : {e}");
            }

            while (true)
            {
                try
                {
                    var response = responseClient.ResponseStream.Current;
                    var sqlCommandResult = response.SqlCommandResult;
                    if (sqlCommandResult == null)
                    {
                        continue;
                    }

                    return new Spark.Connect.Core.Sql.DataFrame.DataFrame(
                        this,
                        sqlCommandResult.Relation
                    );
                }
                catch (Exception e)
                {
                    throw new DataFrameConversionException($"{query} : {e}");
                }
            }
        }

        /// <inheritdoc/>
        public void Stop()
        {
            // Not implemented in spark-connect-go either.
            return;
        }

        /// <summary>
        /// Executes a plan asynchronously.
        /// </summary>
        /// <param name="plan">The plan to execute.</param>
        /// <returns>The asynchronous streaming call for executing the plan.</returns>
        internal AsyncServerStreamingCall<ExecutePlanResponse> ExecutePlan(Plan plan)
        {
            var request = new ExecutePlanRequest
            {
                SessionId = this.SessionId,
                Plan = plan,
                UserContext = new UserContext { UserId = "na", },
            };

            var headers = new Metadata();
            foreach (var entry in this.Metadata)
            {
                headers.Add(entry.Key, entry.Value);
            }

            var callOptions = new CallOptions(headers);

            try
            {
                var responseClient = this.Client.ExecutePlan(request, callOptions);
                return responseClient;
            }
            catch (RpcException e)
            {
                throw new Exception($"Failed to call ExecutePlan in session {this.SessionId}", e);
            }
        }

        /// <summary>
        /// Analyzes the execution plan of a Spark SQL query.
        /// </summary>
        /// <param name="plan">The execution plan to analyze.</param>
        /// <returns>The response containing the analyzed plan.</returns>
        internal AnalyzePlanResponse AnalyzePlan(Plan plan)
        {
            var request = new AnalyzePlanRequest
            {
                SessionId = this.SessionId,
                Schema = new AnalyzePlanRequest.Types.Schema { Plan = plan, },
                UserContext = new UserContext { UserId = "na", },
            };

            var headers = new Metadata();
            foreach (var entry in this.Metadata)
            {
                headers.Add(entry.Key, entry.Value);
            }

            var callOptions = new CallOptions(headers);

            try
            {
                var response = this.Client.AnalyzePlan(request, callOptions);
                return response;
            }
            catch (RpcException e)
            {
                throw new Exception($"Failed to call AnalyzePlan in session {this.SessionId}", e);
            }
        }
    }
}
