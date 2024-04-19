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
        public ICompositeService CompositeService;

        /// <summary>
        /// The Docker host.
        /// </summary>
        public IHostService? DockerHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerComposeTestBase"/> class.
        /// </summary>
        public DockerComposeTestBase()
        {
            this.EnsureDockerHost();
            this.CompositeService = this.Build();
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            this.OnContainerTearDown();
            var compositeService = this.CompositeService;
            this.CompositeService = null!;
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
        /// Start containers.
        /// </summary>
        public virtual void Start()
        {
            try
            {
                this.CompositeService.Start();
            }
            catch
            {
                this.CompositeService.Dispose();
                throw;
            }

            this.OnContainerInitialized();
        }

        /// <summary>
        /// Builds the composite service for Docker Compose tests.
        /// </summary>
        /// <returns>The composite service.</returns>
        public abstract ICompositeService Build();

        /// <summary>
        /// Method called when tearing down the container.
        /// </summary>
        public virtual void OnContainerTearDown() { }

        /// <summary>
        /// Method called when initializing the container.
        /// </summary>
        public virtual void OnContainerInitialized() { }

        /// <summary>
        /// Ensures the Docker host is running.
        /// </summary>
        public void EnsureDockerHost()
        {
            if (this.DockerHost?.State == ServiceRunningState.Running)
            {
                return;
            }

            var hosts = new Hosts().Discover();
            this.DockerHost =
                hosts.FirstOrDefault(x => x.IsNative)
                ?? hosts.FirstOrDefault(x => x.Name == "default");

            if (this.DockerHost != null)
            {
                if (this.DockerHost.State != ServiceRunningState.Running)
                {
                    this.DockerHost.Start();
                }

                return;
            }

            if (hosts.Count > 0)
            {
                this.DockerHost = hosts.First();
            }

            if (this.DockerHost != null)
            {
                return;
            }
        }
    }
}
