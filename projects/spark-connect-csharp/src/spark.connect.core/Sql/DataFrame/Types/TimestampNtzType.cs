// -----------------------------------------------------------------------------
// <summary>
// A TimestampNtz Type.
// </summary>
//
// <copyright file="TimestampNTZType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a TimestampNtz data type in a DataFrame.
    /// </summary>
    public class TimestampNTZType : IDataType
    {
        /// <summary>
        /// Gets the name of the TimestampNtz type.
        /// </summary>
        /// <returns>The name of the TimestampNtz type.</returns>
        public string TypeName() => nameof(TimestampNTZType).Replace("Type", string.Empty);
    }
}
