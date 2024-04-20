// -----------------------------------------------------------------------------
// <summary>
// Response consumer extensions.
// </summary>
//
// <copyright file="ResponseConsumerExtensions.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Grpc.Core;

namespace Spark.Connect.Core.Channel.Extension
{
    /// <summary>
    /// Response consumer extensions.
    /// </summary>
    public static class ResponseConsumerExtensions
    {
        /// <summary>
        /// Consumes the execute plan client response.
        /// </summary>
        /// <param name="responseClient">The response client.</param>
        public static void ConsumeExecutePlanClient(
            this AsyncServerStreamingCall<ExecutePlanResponse> responseClient
        )
        {
            try
            {
                while (responseClient.ResponseStream.MoveNext().Result)
                {
                    // Process the current response.
                    var response = responseClient.ResponseStream.Current;

                    // Here we can process the response if needed. For now,
                    // we'll just discard the response, similar to the Go
                    // implementation.
                }
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.OK)
            {
                // If the RPC call was successful but there's no response, handle it here.
            }
            catch (RpcException e)
            {
                // Handle other RPC exceptions here.
                throw new Exception("Failed to receive plan execution response.", e);
            }
        }
    }
}
