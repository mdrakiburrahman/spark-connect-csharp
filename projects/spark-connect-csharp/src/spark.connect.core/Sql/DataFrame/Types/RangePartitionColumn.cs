// -----------------------------------------------------------------------------
// <summary>
// A column used for range partitioning.
// </summary>
//
// <copyright file="RangePartitionColumn.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a column used for range partitioning.
    /// </summary>
    public record RangePartitionColumn(string Name, bool Descending);
}
