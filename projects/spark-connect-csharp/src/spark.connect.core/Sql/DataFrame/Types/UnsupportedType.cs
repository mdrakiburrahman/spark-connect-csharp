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
    public class UnsupportedType : IDataType
    {
        /// <summary>
        /// Gets the name of the Unsupported type.
        /// </summary>
        /// <returns>The name of the Unsupported type.</returns>
        public string TypeName() => nameof(UnsupportedType).Replace("Type", string.Empty);
    }
}
