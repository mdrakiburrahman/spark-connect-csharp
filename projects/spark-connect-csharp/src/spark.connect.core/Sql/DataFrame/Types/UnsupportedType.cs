// -----------------------------------------------------------------------------
// <summary>
// An Unsupported Type.
// </summary>
//
// <copyright file="UnsupportedType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents an Unsupported data type in a DataFrame.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UnsupportedType"/> class.
    /// </remarks>
    /// <param name="typeInfo">The unsupported type info.</param>
    public class UnsupportedType(object typeInfo) : IDataType
    {
        private object TypeInfo { get; set; } = typeInfo;

        /// <summary>
        /// Gets the name of the Unsupported type.
        /// </summary>
        /// <returns>The name of the Unsupported type.</returns>
        public string TypeName() => nameof(UnsupportedType).Replace("Type", string.Empty);
    }
}
