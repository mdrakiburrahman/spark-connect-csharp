// -----------------------------------------------------------------------------
// <summary>
// Sets up global fixture for the test session.
// </summary>
//
// <copyright file="TestSetup.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Test
{
    /// <summary>
    /// The test setup class.
    /// </summary>
    [TestClass]
    public sealed class TestSetup
    {
        /// <summary>
        /// This method runs on per Test Session spinup.
        /// </summary>
        /// <param name="context">The test context.</param>
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Console.WriteLine($"Setting up global fixture for {context.TestName}");

            // Needed in order to access DogFood environment variables.
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        }

        /// <summary>
        /// This method runs on per Test Session Teardown.
        /// </summary>
        [AssemblyCleanup]
        public static void UnInitialize()
        {
            Console.WriteLine("Tearing down global fixture");
        }
    }
}
