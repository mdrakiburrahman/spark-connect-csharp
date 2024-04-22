// -----------------------------------------------------------------------------
// <summary>
// A simple end-to-end demonstration of Spark Connect C#.
// </summary>
//
// <copyright file="Program.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.DataFrame.Columns;
using Spark.Connect.Core.Sql.Session.Builder;

var demoQuery = "SELECT 'apple' as word, 123 as count UNION ALL SELECT 'orange' as word, 456 as count";
var sparkConnectDockerUri = "sc://host.docker.internal:15002";
var sampleFileTemplate = "file:///tmp/spark-connect-write-{0}-output.parquet";

/* ┌─────────────────────────────┐ *
 * │ Simple in-memory operations │ *
 * └─────────────────────────────┘ */

Console.WriteLine("Initiating Spark Session");
var spark = new SparkSessionBuilder(sparkConnectDockerUri).Build();
Console.WriteLine("");

Console.WriteLine($"Running query: ${demoQuery}");
var inlineDataDf = spark.Sql(demoQuery);
Console.WriteLine("");

Console.WriteLine("DataFrame from query:");
inlineDataDf.Show(5, false);
Console.WriteLine("");

Console.WriteLine("DataFrame Spark Schema:");
var sparkSchema = inlineDataDf.Schema();
foreach (var field in sparkSchema.Fields) Console.WriteLine($"Field: {field.Name}, Type: {field.DataType}");
Console.WriteLine("");

/* ┌────────────────────────────────┐ *
 * │ Pull locally into Apache Arrow │ *
 * └────────────────────────────────┘ */

Console.WriteLine("Collecting all rows from Connect Server in DataFrame into Apache Arrow format");
var rows = inlineDataDf.Collect();

foreach (var row in rows)
{
    var arrowSchema = row.Schema();
    var arrowValues = row.Values();

    for (var i = 0; i < arrowValues.Length; i++)
    {
        Console.WriteLine(
            $"Column Name: {arrowSchema?.Fields[i].Name}, Value: {arrowValues[i]}, Type: {arrowSchema?.Fields[i].DataType}"
        );
    }
}
Console.WriteLine("");

/* ┌────────────┐ *
 * │ Read/Write │ *
 * └────────────┘ */

Console.WriteLine("Writing to Parquet");
inlineDataDf.Write().Mode("overwrite").Format("parquet").Save(string.Format(sampleFileTemplate, "sample"));
Console.WriteLine("");

Console.WriteLine("Reading from Parquet");
spark.Read().Format("parquet").Load(string.Format(sampleFileTemplate, "sample")).Show(5, false);
Console.WriteLine("");

Console.WriteLine("Creating Temporary View");
inlineDataDf.CreateTempView("fruit_counts");
Console.WriteLine("");

Console.WriteLine("Querying Temporary View");
spark.Sql("SELECT * FROM fruit_counts").Show(5, false);
Console.WriteLine("");

/* ┌──────────────┐ *
 * │ Partitioning │ *
 * └──────────────┘ */

Console.WriteLine("Writing with various partitioning techniques");
inlineDataDf.Repartition(1, new string[] { }).Write().Mode("overwrite").Format("parquet").Save(string.Format(sampleFileTemplate, "one"));
inlineDataDf.Repartition(2, new string[] { }).Write().Mode("overwrite").Format("parquet").Save(string.Format(sampleFileTemplate, "two"));
inlineDataDf.Repartition(0, new string[] { "word", "count" }).Write().Mode("overwrite").Format("parquet").Save(string.Format(sampleFileTemplate, "with-column"));
inlineDataDf.RepartitionByRange(0, [new RangePartitionColumn("word", true)]).Write().Mode("overwrite").Format("parquet").Save(string.Format(sampleFileTemplate, "with-range"));
Console.WriteLine("");

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done!");
Console.ResetColor();
