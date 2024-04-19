// -----------------------------------------------------------------------------
// <summary>
// Docker compose base class for tests.
// </summary>
//
// <copyright file="DockerComposeTestBase.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Test.Common.TestHost
{
    using Ductus.FluentDocker.Services;

    /// <summary>
    /// Abstract class for Docker Compose tests.
    /// </summary>
    public abstract class DockerComposeTestBase : IDisposable
    {
        /// <summary>
        /// The Docker host composite service.
        /// </summary>
        protected ICompositeService compositeService;

        /// <summary>
        /// The Docker host.
        /// </summary>
        protected IHostService? dockerHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerComposeTestBase"/> class.
        /// </summary>
        public DockerComposeTestBase()
        {
            this.EnsureDockerHost();
            this.compositeService = this.Build();
            try
            {
                this.compositeService.Start();
            }
            catch
            {
                this.compositeService.Dispose();
                throw;
            }

            this.OnContainerInitialized();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.OnContainerTearDown();
            var compositeService = this.compositeService;
            this.compositeService = null!;
            try
            {
                compositeService?.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Builds the composite service for Docker Compose tests.
        /// </summary>
        /// <returns>The composite service.</returns>
        protected abstract ICompositeService Build();

        /// <summary>
        /// Method called when tearing down the container.
        /// </summary>
        protected virtual void OnContainerTearDown() { }

        /// <summary>
        /// Method called when initializing the container.
        /// </summary>
        protected virtual void OnContainerInitialized() { }

        private void EnsureDockerHost()
        {
            if (this.dockerHost?.State == ServiceRunningState.Running)
            {
                return;
            }

            var hosts = new Hosts().Discover();
            this.dockerHost =
                hosts.FirstOrDefault(x => x.IsNative)
                ?? hosts.FirstOrDefault(x => x.Name == "default");

            if (this.dockerHost != null)
            {
                if (this.dockerHost.State != ServiceRunningState.Running)
                {
                    this.dockerHost.Start();
                }

                return;
            }

            if (hosts.Count > 0)
            {
                this.dockerHost = hosts.First();
            }

            if (this.dockerHost != null)
            {
                return;
            }
        }
    }
}
