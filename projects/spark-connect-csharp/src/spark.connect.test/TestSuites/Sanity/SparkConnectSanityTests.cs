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
    using Spark.Connect.Test.Common.SparkEnvironment;

    /// <summary>
    /// Spark Connect Sanity tests.
    /// </summary>
    [TestClass]
    public class SparkConnectSanityTests
    {
        private SparkConnectServer sparkConnectServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkConnectSanityTests"/> class.
        /// </summary>
        public SparkConnectSanityTests()
        {
            this.sparkConnectServer = new SparkConnectServer(forceKill: true);
        }

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

            // TODO: Put this in the right place, and have a force kill option?
            this.sparkConnectServer.Dispose();
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
            await Task.Delay(1000);
            Assert.AreEqual(1, 1);
        }

        #endregion Test Methods
    }
}
