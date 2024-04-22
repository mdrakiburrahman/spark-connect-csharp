// -----------------------------------------------------------------------------
// <summary>
// A Boolean Type.
// </summary>
//
// <copyright file="BooleanType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Boolean data type in a DataFrame.
    /// </summary>
    public class BooleanType : IDataType
    {
        /// <summary>
        /// Gets the name of the Boolean type.
        /// </summary>
        /// <returns>The name of the Boolean type.</returns>
        public string TypeName() => nameof(BooleanType).Replace("Type", string.Empty);
    }
}
