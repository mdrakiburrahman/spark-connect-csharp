// -----------------------------------------------------------------------------
// <summary>
// A simple end-to-end demonstration of Spark Connect in C#.
// </summary>
//
// <copyright file="Program.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Demo
{
    /// <summary>
    /// A simple end-to-end demonstration of Spark Connect in C#.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The default Spark Connect URI when running in a Docker container.
        /// </summary>
        public static readonly string SparkConnectUri = "sc://host.docker.internal:15002";

        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <param name="args">Command line Arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
