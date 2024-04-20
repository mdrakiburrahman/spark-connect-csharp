// -----------------------------------------------------------------------------
// <summary>
// A gRPC Channel Builder.
// </summary>
//
// <copyright file="ChannelBuilder.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Grpc.Core;

using Spark.Connect.Core.Channel.Auth;

using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Spark.Connect.Core.Channel.Builder
{
    /// <summary>
    /// A gRPC Channel Builder.
    /// </summary>
    internal class ChannelBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelBuilder"/> class.
        /// </summary>
        /// <param name="connection">The connection string.</param>
        public ChannelBuilder(string connection)
        {
            var u = new Uri(connection);

            if (u.Scheme != "sc")
            {
                throw new ArgumentException("URL schema must be set to `sc`.");
            }

            this.Port = u.Port == -1 ? 15002 : u.Port;
            this.Host = u.Host;
            this.Token = string.Empty;
            this.User = string.Empty;

            var queryParts = u.Query.Split(';');
            this.Headers = new Dictionary<string, string>();
            foreach (var part in queryParts)
            {
                var parts = part.Split('=');
                if (parts.Length == 2)
                {
                    switch (parts[0])
                    {
                        case "token":
                            this.Token = parts[1];
                            break;
                        case "user_id":
                            this.User = parts[1];
                            break;
                        default:
                            this.Headers[parts[0]] = parts[1];
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the host of the channel.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Gets the port of the channel.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets the token for authentication.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public string User { get; private set; }

        /// <summary>
        /// Gets the headers for the channel.
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// Builds the gRPC channel.
        /// </summary>
        /// <returns>The gRPC channel.</returns>
        public Grpc.Core.Channel Build()
        {
            var options = new List<ChannelOption>
            {
                new ChannelOption(ChannelOptions.SslTargetNameOverride, this.Host),
            };

            Grpc.Core.ChannelCredentials channelCredentials;

            if (string.IsNullOrEmpty(this.Token))
            {
                channelCredentials = ChannelCredentials.Insecure;
            }
            else
            {
                var systemRoots = new X509Certificate2Collection();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    systemRoots = this.GetSystemRoots();
                }

                var credentials = new SslCredentials();
                var callCredentials = CallCredentials.FromInterceptor(
                    (context, metadata) =>
                    {
                        return Task.CompletedTask;
                    }
                );

                channelCredentials = ChannelCredentials.Create(credentials, callCredentials);
            }

            var channel = new Grpc.Core.Channel(this.Host, this.Port, channelCredentials);

            return channel;
        }

        /// <summary>
        /// Gets the system root certificates.
        /// </summary>
        /// <returns>The system root certificates.</returns>
        private X509Certificate2Collection GetSystemRoots()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = store.Certificates;
            store.Close();
            return certificates;
        }
    }
}
