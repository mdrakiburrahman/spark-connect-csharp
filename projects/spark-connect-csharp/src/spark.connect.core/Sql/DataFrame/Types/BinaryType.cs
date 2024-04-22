// -----------------------------------------------------------------------------
// <summary>
// A Binary Type.
// </summary>
//
// <copyright file="BinaryType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Binary data type in a DataFrame.
    /// </summary>
    public class BinaryType : IDataType
    {
        /// <summary>
        /// Gets the name of the Binary type.
        /// </summary>
        /// <returns>The name of the Binary type.</returns>
        public string TypeName() => nameof(BinaryType).Replace("Type", string.Empty);
    }
}
