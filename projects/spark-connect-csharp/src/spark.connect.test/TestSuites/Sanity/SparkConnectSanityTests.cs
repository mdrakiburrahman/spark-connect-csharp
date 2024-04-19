// -----------------------------------------------------------------------------
// <summary>
// Contains Basic Sanity Tests for Spark Connect.
// </summary>
//
// <copyright file="SparkConnectSanityTests.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Test.TestSuites.Sanity
{
    using DotNet.Testcontainers.Builders;

    /// <summary>
    /// Spark Connect Sanity tests.
    /// </summary>
    [TestClass]
    public class SparkConnectSanityTests
    {
        #region Test Properties

        private const string TestContainerImage = "testcontainers/helloworld:1.1.0";

        #endregion Test Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkConnectSanityTests"/> class.
        /// </summary>
        public SparkConnectSanityTests() { }

        #region Class scoped fixture

        /// <summary>
        /// This method runs on per Test Class spinup.
        /// </summary>
        /// <param name="context">The test context.</param>
        [ClassInitialize]
        public static void SparkConnectSanityTestsInit(TestContext context)
        {
            Console.WriteLine($"Setting up class scoped fixture for test {context.TestName}");
        }

        /// <summary>
        /// This method runs on per Test Class Teardown.
        /// </summary>
        [ClassCleanup]
        public static void SqlServerDatabaseTestsClassTeardown()
        {
            Console.WriteLine("Tearing down class scoped fixture");
        }

        #endregion Class scoped fixture

        #region Test scoped fixture

        /// <summary>
        /// This method runs on per Test Method spinup.
        /// </summary>
        [TestInitialize]
        public void SqlServerDatabaseTestsMethodInit()
        {
            Console.WriteLine("Setting up method scoped fixture");
        }

        /// <summary>
        /// This method runs on per Test Method teardown.
        /// </summary>
        [TestCleanup]
        public void SqlServerDatabaseTestsMethodTeardown()
        {
            Console.WriteLine("Tearing down method scoped fixture");
        }

        #endregion Test scoped fixture

        #region Test Methods

        /// <summary>
        /// Ensure Spark Connect can connect to the Spark cluster.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task SparkConnectCanConnectAsync()
        {
            var container = new ContainerBuilder()
                .WithImage("testcontainers/helloworld:1.1.0")
                .WithPortBinding(8080, true)
                .WithWaitStrategy(
                    Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080))
                )
                .Build();

            await container.StartAsync().ConfigureAwait(false);

            var httpClient = new HttpClient();
            var requestUri = new UriBuilder(
                Uri.UriSchemeHttp,
                container.Hostname,
                container.GetMappedPublicPort(8080),
                "uuid"
            ).Uri;
            var guid = await httpClient.GetStringAsync(requestUri).ConfigureAwait(false);

            Console.WriteLine($"Received GUID: {guid}");

            Assert.AreEqual(1, 1);
        }

        #endregion Test Methods
    }
}
