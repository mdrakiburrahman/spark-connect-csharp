// -----------------------------------------------------------------------------
// <summary>
// A contract for Building a Spark Session.
// </summary>
//
// <copyright file="ISparkSessionBuilder.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.Session.Builder
{
    /// <summary>
    /// A contract for Building a Spark Session.
    /// </summary>
    public interface ISparkSessionBuilder
    {
        /// <summary>
        /// Builds the Spark Session.
        /// </summary>
        /// <returns>The built Spark Session.</returns>
        ISparkSession Build();
    }
}
