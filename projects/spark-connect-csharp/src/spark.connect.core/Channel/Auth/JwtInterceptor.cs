// -----------------------------------------------------------------------------
// <summary>
// An internal JWT interceptor.
// </summary>
//
// <copyright file="JwtInterceptor.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Spark.Connect.Core.Channel.Auth
{
    /// <summary>
    /// A JWT interceptor.
    /// </summary>
    internal class JwtInterceptor : Interceptor
    {
        private readonly string token;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtInterceptor"/> class.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        public JwtInterceptor(string token)
        {
            this.token = token;
        }

        /// <inheritdoc/>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation
        )
        {
            var headers = new Metadata { { "Authorization", $"Bearer {this.token}" } };

            var mergedHeaders = new Metadata();
            foreach (var entry in context.Options.Headers ?? new Metadata())
            {
                mergedHeaders.Add(entry);
            }

            foreach (var entry in headers)
            {
                mergedHeaders.Add(entry);
            }

            var options = context.Options.WithHeaders(mergedHeaders);

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                options
            );

            return base.AsyncUnaryCall(request, newContext, continuation);
        }
    }
}
