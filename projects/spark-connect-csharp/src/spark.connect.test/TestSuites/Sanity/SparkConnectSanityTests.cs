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

using Spark.Connect.Core.Sql.Session;
using Spark.Connect.Core.Sql.Session.Builder;
using Spark.Connect.Test.Common.SparkEnvironment;

using System;

namespace Spark.Connect.Test.TestSuites.Sanity
{
    /// <summary>
    /// Spark Connect Sanity tests.
    /// </summary>
    [TestClass]
    public class SparkConnectSanityTests
    {
        private static readonly SparkConnectServer SparkConnectServer = new SparkConnectServer(
            forceKill: true
        );

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
            SparkConnectServer.Start();
        }

        /// <summary>
        /// This method runs on per Test Class Teardown.
        /// </summary>
        [ClassCleanup]
        public static void SqlServerDatabaseTestsClassTeardown()
        {
            Console.WriteLine("Tearing down class scoped fixture");
            SparkConnectServer.Dispose();
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

        // csharpier-ignore-start
        /// <summary>
        /// Asserts Spark Session can build as expected.
        /// </summary>
        /// <param name="url">The Spark Connect Server url.</param>
        /// <param name="shouldThrow">Should throw during initialization.</param>
        /// <param name="e">Thrown exception type.</param>
        [TestMethod]
        [DataRow("sc://host.docker.internal:15002", false, null)]
        [DataRow("sc://host.docker.internal:15002/;user_id=a;token=b;x-other-header=c", false, null)]
        [DataRow("sc://host.docker.internal:123", false, null)]
        [DataRow("sc://host.docker.internal:123/;user_id=a;token=b;x-other-header=c", false, null)]
        [DataRow("sc://host.docker.internal", false, null)]
        [DataRow("sc://host.docker.internal/;user_id=a;token=b;x-other-header=c", false, null)]
        [DataRow("sc://host.docker.internal:port", true, "UriFormatException")]
        [DataRow("sc://host.docker.internal:port/;user_id=a;token=b;x-other-header=c", true, "UriFormatException")]
        [DataRow("host.docker.internal:15002", true, "ArgumentException")]
        [DataRow("host.docker.internal:15002;user_id=a;token=b;x-other-header=c", true, "ArgumentException")]
        [DataRow("abc://host.docker.internal:15002", true, "ArgumentException")]
        [DataRow("abc://host.docker.internal:15002/;user_id=a;token=b;x-other-header=c", true, "ArgumentException")]
        public void TestSparkSessionCanBuildAsExpected(string url, bool shouldThrow, string e)
        {
            if (shouldThrow)
            {
                switch (e)
                {
                    case "UriFormatException":
                        Assert.ThrowsException<UriFormatException>(
                            () => this.CreateSparkSession(url)
                        );
                        break;
                    default:
                        Assert.ThrowsException<ArgumentException>(
                            () => this.CreateSparkSession(url)
                        );
                        break;
                }
            }
            else
            {
                var spark = this.CreateSparkSession(url);
                Assert.IsNotNull(spark);
            }
        }
        // csharpier-ignore-end

        /// <summary>
        /// Asserts queries return expected results.
        /// </summary>
        /// <param name="query">The query to run.</param>
        /// <param name="expectedOutputs">The expected outputs.</param>
        /// <param name="shouldThrow">Should throw during initialization.</param>
        /// <param name="e">Thrown exception type.</param>
        [TestMethod]
        [DataRow(
            "select 'apple' as word, 123 as count union all select 'orange' as word, 456 as count",
            new string[] { "orange", "456", "apple", "123" },
            false,
            null
        )]
        [DataRow(
            "select 'potato' as word, 999999999 as count union all select 'tomato' as word, 0 as count",
            new string[] { "potato", "999999999", "tomato", "0" },
            false,
            null
        )]
        [DataRow(
            "foo",
            null,
            true,
            "AggregateException"
        )]
        public void TestSparkQueryCanRunAsExpected(
            string query,
            string[] expectedOutputs,
            bool shouldThrow,
            string e
        )
        {
            // Setup
            //
            var spark = this.CreateSparkSession();
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            if (shouldThrow)
            {
                switch (e)
                {
                    case "AggregateException":
                        Assert.ThrowsException<AggregateException>(
                            () => spark.Sql(query)
                        );
                        break;
                    default:
                        Assert.ThrowsException<ArgumentException>(
                            () => spark.Sql(query)
                        );
                        break;
                }
            }
            else
            {
                var df = spark.Sql(query);

                // Exercise
                //
                df.Show();

                // Verify
                //
                var output = consoleOutput.ToString();
                Assert.IsTrue(expectedOutputs.All(o => output.Contains(o)));

                // Clean up
                //
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        /// <summary>
        /// Creates a new instance of the Spark session.
        /// </summary>
        /// <param name="url">The Spark Connect Server url.</param>
        /// <returns>The Spark session.</returns>
        private ISparkSession CreateSparkSession(string url = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                return new SparkSessionBuilder(SparkConnectServer.SparkConnectUri).Build();
            }

            return new SparkSessionBuilder(url).Build();
        }

        #endregion Private Helper Methods
    }
}
