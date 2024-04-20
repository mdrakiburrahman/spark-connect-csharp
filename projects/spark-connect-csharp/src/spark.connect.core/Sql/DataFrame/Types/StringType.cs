// -----------------------------------------------------------------------------
// <summary>
// A String Type.
// </summary>
//
// <copyright file="StringType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a String data type in a DataFrame.
    /// </summary>
    public class StringType : IDataType
    {
        /// <summary>
        /// Gets the name of the String type.
        /// </summary>
        /// <returns>The name of the String type.</returns>
        public string TypeName() => nameof(StringType).Replace("Type", string.Empty);
    }
}
