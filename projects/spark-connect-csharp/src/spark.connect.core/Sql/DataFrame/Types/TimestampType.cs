// -----------------------------------------------------------------------------
// <summary>
// A Timestamp Type.
// </summary>
//
// <copyright file="TimestampType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Timestamp data type in a DataFrame.
    /// </summary>
    public class TimestampType : IDataType
    {
        /// <summary>
        /// Gets the name of the Timestamp type.
        /// </summary>
        /// <returns>The name of the Timestamp type.</returns>
        public string TypeName() => nameof(TimestampType).Replace("Type", string.Empty);
    }
}
