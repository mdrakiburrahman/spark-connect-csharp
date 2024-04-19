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
        /// <summary>
        /// Initializes a new instance of the <see cref="SparkConnectServer"/> class.
        /// </summary>
        public SparkConnectServer() { }

        /// <inheritdoc/>
        protected override ICompositeService Build()
        {
            var file = Path.Combine(
                Directory.GetCurrentDirectory(),
                (TemplateString)"Common/SparkHost/docker-compose-spark-connect-server.yaml"
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
    }
}
