// -----------------------------------------------------------------------------
// <summary>
// A contract for data types.
// </summary>
//
// <copyright file="IDataType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a data type.
    /// </summary>
    public interface IDataType
    {
        /// <summary>
        /// Gets the name of the data type.
        /// </summary>
        /// <returns>The name of the data type.</returns>
        string TypeName();
    }
}
