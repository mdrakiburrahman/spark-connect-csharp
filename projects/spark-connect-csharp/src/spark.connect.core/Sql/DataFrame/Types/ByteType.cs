// -----------------------------------------------------------------------------
// <summary>
// A Byte Type.
// </summary>
//
// <copyright file="ByteType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Byte data type in a DataFrame.
    /// </summary>
    public class ByteType : IDataType
    {
        /// <summary>
        /// Gets the name of the Byte type.
        /// </summary>
        /// <returns>The name of the Byte type.</returns>
        public string TypeName() => nameof(ByteType).Replace("Type", string.Empty);
    }
}
