// -----------------------------------------------------------------------------
// <summary>
// A structured data type.
// </summary>
//
// <copyright file="StructType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a structured data type.
    /// </summary>
    public record StructType(string TypeName, StructField[] Fields);
}
