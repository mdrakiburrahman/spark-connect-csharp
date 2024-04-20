// -----------------------------------------------------------------------------
// <summary>
// A Short Type.
// </summary>
//
// <copyright file="ShortType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Short data type in a DataFrame.
    /// </summary>
    public class ShortType : IDataType
    {
        /// <summary>
        /// Gets the name of the Short type.
        /// </summary>
        /// <returns>The name of the Short type.</returns>
        public string TypeName() => nameof(ShortType).Replace("Type", string.Empty);
    }
}
