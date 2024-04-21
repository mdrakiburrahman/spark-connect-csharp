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

using Newtonsoft.Json;

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

        private static readonly string DemoQuery =
            "SELECT 'apple' as word, 123 as count UNION ALL SELECT 'orange' as word, 456 as count";

        private static readonly string[] DemoQueryExpectedOutputs = new string[]
        {
            "apple",
            "123",
            "orange",
            "456",
        };

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
        public void SparkConnectSanityTestsMethodInit()
        {
            Console.WriteLine("Setting up method scoped fixture");
        }

        /// <summary>
        /// This method runs on per Test Method teardown.
        /// </summary>
        [TestCleanup]
        public void SparkConnectSanityTestsMethodTeardown()
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
            "SELECT 'apple' as word, 123 as count UNION ALL SELECT 'orange' as word, 456 as count",
            new string[] { "orange", "456", "apple", "123" },
            false,
            null
        )]
        [DataRow(
            "SELECT 'potato' as word, 999999999 as count UNION ALL SELECT 'tomato' as word, 0 as count",
            new string[] { "potato", "999999999", "tomato", "0" },
            false,
            null
        )]
        [DataRow("foo", null, true, "AggregateException")]
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
                        Assert.ThrowsException<AggregateException>(() => spark.Sql(query));
                        break;
                    default:
                        Assert.ThrowsException<ArgumentException>(() => spark.Sql(query));
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
        /// Asserts queries return expected results.
        /// </summary>
        /// <param name="query">The query to run.</param>
        /// <param name="expectedColumnNames">The expected column names.</param>
        /// <param name="expectedDataTypes">The expected data types.</param>
        [TestMethod]
        [DataRow(
            "SELECT 'apple' as word, 123 as count UNION ALL SELECT 'orange' as word, 456 as count",
            new string[] { "word", "count" },
            new string[] { "StringType", "IntegerType" }
        )]
        [DataRow(
            @"SELECT 
                true AS is_fruit, 
                123 AS fruit_id, 
                CURRENT_TIMESTAMP() AS purchase_timestamp,
                'apple' AS fruit_name, 
                9999999929299292 AS num_purchases, 
                3.487947484763867328929829272 AS price",
            new string[]
            {
                "is_fruit",
                "fruit_id",
                "purchase_timestamp",
                "fruit_name",
                "num_purchases",
                "price",
            },
            new string[]
            {
                "BooleanType",
                "IntegerType",
                "TimestampType",
                "StringType",
                "LongType",
                "DecimalType",
            }
        )]
        public void TestSparkSchemaReturnsAsExpected(
            string query,
            string[] expectedColumnNames,
            string[] expectedDataTypes
        )
        {
            // Setup
            //
            var spark = this.CreateSparkSession();
            var df = spark.Sql(query);

            // Exercise
            //
            var schema = df.Schema();

            // Verify
            //
            for (int i = 0; i < expectedColumnNames.Length; i++)
            {
                var field = schema.Fields.FirstOrDefault(f => f.Name == expectedColumnNames[i]);
                Assert.IsNotNull(field, $"Field {expectedColumnNames[i]} not found in schema.");
                Assert.AreEqual(
                    $"Spark.Connect.Core.Sql.DataFrame.Types.{expectedDataTypes[i]}",
                    field.DataType.ToString()
                );
            }
        }

        /// <summary>
        /// Asserts queries return expected results.
        /// </summary>
        /// <param name="query">The query to run.</param>
        /// <param name="expectedColumnNames">The expected column names.</param>
        /// <param name="expectedRowsJson">The expected rows in JSON format.</param>
        /// <param name="expectedColumnTypes">The expected column types.</param>
        [TestMethod]
        [DataRow(
            "SELECT 'apple' as word, 123 as count UNION ALL SELECT 'orange' as word, 456 as count",
            "word,count",
            "[[\"apple\",\"123\"],[\"orange\",\"456\"]]",
            "StringType,IntegerType"
        )]
        public void TestSparkRowCollectionReturnsAsExpected(
            string query,
            string expectedColumnNames,
            string expectedRowsJson,
            string expectedColumnTypes
        )
        {
            // Convert CSV and JSON parameters to arrays
            string[] expectedColumnNamesArray = expectedColumnNames.Split(',');

#pragma warning disable CS8600
            string[][] expectedRows = JsonConvert.DeserializeObject<string[][]>(expectedRowsJson);
#pragma warning restore CS8600
            string[] expectedColumTypesArray = expectedColumnTypes.Split(',');

            // Setup
            //
            var spark = this.CreateSparkSession();
            var df = spark.Sql(query);

            // Exercise
            //
            var rows = df.Collect();

            // Verify
            //
            for (int i = 0; i < expectedRows?.Length; i++)
            {
                var row = rows[i];
                var schema = row.Schema();
                var values = row.Values();

                for (int j = 0; j < expectedColumnNamesArray.Length; j++)
                {
                    var field = schema?.Fields.FirstOrDefault(
                        f => f.Name == expectedColumnNamesArray[j]
                    );
                    Assert.IsNotNull(
                        field,
                        $"Field {expectedColumnNamesArray[j]} not found in schema."
                    );
                    Assert.AreEqual(
                        $"Spark.Connect.Core.Sql.DataFrame.Types.{expectedColumTypesArray[j]}",
                        field.DataType.ToString()
                    );
                    Assert.AreEqual(expectedRows[i][j], values[j].ToString());
                }
            }
        }

        /// <summary>
        /// Asserts DataFrame writes can be read as expected.
        /// </summary>
        [TestMethod]
        public void TestDataFrameWriteReadAsExpected()
        {
            // Setup
            //
            var sampleFile = "file:///tmp/spark-connect-write-example-output.parquet";

            var spark = this.CreateSparkSession();
            var inMemDf = spark.Sql(DemoQuery);

            // Exercise
            //
            inMemDf.Write().Mode("overwrite").Format("parquet").Save(sampleFile);

            // Verify
            //
            var storageDf = spark.Read().Format("parquet").Load(sampleFile);
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            storageDf.Show();

            // Verify
            //
            var output = consoleOutput.ToString();
            Assert.IsTrue(DemoQueryExpectedOutputs.All(o => output.Contains(o)));

            // Clean up
            //
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
        }

        /// <summary>
        /// Asserts Temporary Views can be read as expected.
        /// </summary>
        [TestMethod]
        public void TestTempViewReadAsExpected()
        {
            // Setup
            //
            var spark = this.CreateSparkSession();
            var inMemDf = spark.Sql(DemoQuery);

            // Exercise
            //
            inMemDf.CreateTempView("fruit_sales");

            // Verify
            //
            var tempViewDf = spark.Sql("select count, word from fruit_sales order by count");
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            tempViewDf.Show();

            // Verify
            //
            var output = consoleOutput.ToString();
            Assert.IsTrue(DemoQueryExpectedOutputs.All(o => output.Contains(o)));

            // Clean up
            //
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
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
