// -----------------------------------------------------------------------------
// <summary>
// A row in a data frame.
// </summary>
//
// <copyright file="IRow.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.DataFrame.Types;

namespace Spark.Connect.Core.Sql.DataFrame.Rows
{
    /// <summary>
    /// Represents a row in a data frame.
    /// </summary>
    public interface IRow
    {
        /// <summary>
        /// Returns the schema for the row.
        /// </summary>
        /// <returns>The schema for the row.</returns>
        StructType? Schema();

        /// <summary>
        /// Returns the values of the row.
        /// </summary>
        /// <returns>The values of the row.</returns>
        object[] Values();
    }
}
