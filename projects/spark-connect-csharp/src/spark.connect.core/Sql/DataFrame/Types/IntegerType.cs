// -----------------------------------------------------------------------------
// <summary>
// An Integer Type.
// </summary>
//
// <copyright file="IntegerType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Integer data type in a DataFrame.
    /// </summary>
    public class IntegerType : IDataType
    {
        /// <summary>
        /// Gets the name of the Integer type.
        /// </summary>
        /// <returns>The name of the Integer type.</returns>
        public string TypeName() => nameof(IntegerType).Replace("Type", string.Empty);
    }
}
