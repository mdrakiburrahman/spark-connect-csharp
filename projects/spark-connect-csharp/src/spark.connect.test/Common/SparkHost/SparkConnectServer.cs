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

namespace Spark.Connect.Test.Common.SparkEnvironment
{
    using Ductus.FluentDocker.Commands;
    using Ductus.FluentDocker.Model.Common;
    using Ductus.FluentDocker.Model.Compose;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Impl;

    using Spark.Connect.Test.Common.TestHost;

    /// <summary>
    /// A Spark Connect Server.
    /// </summary>
    public class SparkConnectServer : DockerComposeTestBase
    {
        private string composeFilePath =
            "Common/SparkHost/docker-compose-spark-connect-server.yaml";

        private bool forceKill = true;

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
            base.Dispose();
        }

        /// <inheritdoc/>
        protected override ICompositeService Build()
        {
            var file = Path.Combine(
                Directory.GetCurrentDirectory(),
                (TemplateString)this.composeFilePath
            );

            return new DockerComposeCompositeService(
                this.dockerHost,
                new DockerComposeConfig
                {
                    ComposeFilePath = new List<string> { file },
                    ForceRecreate = true,
                    RemoveOrphans = true,
                    StopOnDispose = true,
                }
            );
        }

        /// <inheritdoc/>
        protected override void OnContainerTearDown()
        {
            if (this.forceKill)
            {
                Compose.ComposeKill(
                    host: this.dockerHost?.Host,
                    composeFile: Path.Combine(
                        Directory.GetCurrentDirectory(),
                        (TemplateString)this.composeFilePath
                    )
                );
            }

            base.OnContainerTearDown();
        }
    }
}
