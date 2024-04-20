// -----------------------------------------------------------------------------
// <summary>
// A structured field.
// </summary>
//
// <copyright file="StructField.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.DataFrame.Types;

namespace Spark.Connect.Core.Sql.DataFrame.Fields
{
    /// <summary>
    /// Represents a field in a structured data type.
    /// </summary>
    public record StructField(string Name, IDataType DataType, bool Nullable = true);
}
