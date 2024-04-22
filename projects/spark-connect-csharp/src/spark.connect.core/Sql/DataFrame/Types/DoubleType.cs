// -----------------------------------------------------------------------------
// <summary>
// A Double Type.
// </summary>
//
// <copyright file="DoubleType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Double data type in a DataFrame.
    /// </summary>
    public class DoubleType : IDataType
    {
        /// <summary>
        /// Gets the name of the Double type.
        /// </summary>
        /// <returns>The name of the Double type.</returns>
        public string TypeName() => nameof(DoubleType).Replace("Type", string.Empty);
    }
}
