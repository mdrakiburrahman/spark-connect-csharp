// -----------------------------------------------------------------------------
// <summary>
// A Plan ID Generator.
// </summary>
//
// <copyright file="PlanIdGenerator.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------
namespace Spark.Connect.Core.Sql
{
    /// <summary>
    /// Generates plan IDs.
    /// </summary>
    public static class PlanIdGenerator
    {
        private static long atomicInt64;

        /// <summary>
        /// Create a new plan ID.
        /// </summary>
        /// <returns>A new plan ID.</returns>
        public static long NewPlanId()
        {
            long v = Interlocked.Increment(ref atomicInt64);
            return v;
        }
    }
}
