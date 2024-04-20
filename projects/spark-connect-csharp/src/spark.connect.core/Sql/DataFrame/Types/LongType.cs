// -----------------------------------------------------------------------------
// <summary>
// A Long Type.
// </summary>
//
// <copyright file="LongType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Long data type in a DataFrame.
    /// </summary>
    public class LongType : IDataType
    {
        /// <summary>
        /// Gets the name of the Long type.
        /// </summary>
        /// <returns>The name of the Long type.</returns>
        public string TypeName() => nameof(LongType).Replace("Type", string.Empty);
    }
}
