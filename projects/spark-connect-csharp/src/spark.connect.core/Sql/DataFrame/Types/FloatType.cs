// -----------------------------------------------------------------------------
// <summary>
// A Float Type.
// </summary>
//
// <copyright file="FloatType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Float data type in a DataFrame.
    /// </summary>
    public class FloatType : IDataType
    {
        /// <summary>
        /// Gets the name of the Float type.
        /// </summary>
        /// <returns>The name of the Float type.</returns>
        public string TypeName() => nameof(FloatType).Replace("Type", string.Empty);
    }
}
