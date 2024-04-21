// -----------------------------------------------------------------------------
// <summary>
// Sets up a Spark Connect Server.
// </summary>
//
// <copyright file="SparkConnectServer.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Model.Compose;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;

using Spark.Connect.Test.Common.TestHost;

namespace Spark.Connect.Test.Common.SparkEnvironment
{
    /// <summary>
    /// A Spark Connect Server.
    /// </summary>
    public class SparkConnectServer : DockerComposeTestBase
    {
        /// <summary>
        /// The Spark Master URI.
        /// </summary>
        public readonly string SparkMasterUri = "http://host.docker.internal:4040";

        /// <summary>
        /// The Spark Connect URI.
        /// </summary>
        public readonly string SparkConnectUri = "sc://host.docker.internal:15002";

        private readonly string composeFilePath =
            "Common/SparkHost/docker-compose-spark-connect-server.yaml";

        private readonly int sparkReadinessTimeoutInSeconds = 60;

        private readonly bool forceKill;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkConnectServer"/> class.
        /// </summary>
        /// <param name="forceKill">Force kill the container on teardown.</param>
        public SparkConnectServer(bool forceKill = true)
        {
            this.forceKill = forceKill;
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        /// <inheritdoc/>
        public override void Start()
        {
            base.Start();
        }

        /// <inheritdoc/>
        public override ICompositeService Build()
        {
            var file = GetComposeFile(this.composeFilePath);

            // Clean up any existing containers.
            this.ForceKill(file);

            return new DockerComposeCompositeService(
                this.DockerHost,
                new DockerComposeConfig
                {
                    ComposeFilePath = new List<string> { file },
                    ForceRecreate = true,
                    RemoveOrphans = true,
                    StopOnDispose = true,
                    UseColor = true,
                }
            );
        }

        /// <inheritdoc/>
        public override void OnContainerTearDown()
        {
            if (this.forceKill)
            {
                this.ForceKill(GetComposeFile(this.composeFilePath));
            }

            base.OnContainerTearDown();
        }

        /// <inheritdoc/>
        public override void OnContainerInitialized()
        {
            Task.Run(async () =>
                {
                    var startTime = DateTime.Now;

                    while (string.IsNullOrEmpty(await this.SparkMasterUri.Wget()))
                    {
                        if (
                            (DateTime.Now - startTime).TotalSeconds
                            > this.sparkReadinessTimeoutInSeconds
                        )
                        {
                            throw new Exception(
                                $"Timeout: Spark Master Server did not start within {this.sparkReadinessTimeoutInSeconds} seconds."
                            );
                        }

                        Console.WriteLine("Waiting for Spark Master Server to start...");
                        await Task.Delay(1000);
                    }
                })
                .Wait();

            base.OnContainerInitialized();
        }

        /// <summary>
        /// Gets the full path of the compose file.
        /// </summary>
        /// <param name="filePath">The relative file path.</param>
        /// <returns>The full path of the compose file.</returns>
        private static string GetComposeFile(string filePath)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)filePath);
        }

        /// <summary>
        /// Forces the kill of the Spark Connect Server container.
        /// </summary>
        private void ForceKill(string? filePath)
        {
            this.DockerHost?.Host.ComposeKill(composeFile: filePath);
        }
    }
}
