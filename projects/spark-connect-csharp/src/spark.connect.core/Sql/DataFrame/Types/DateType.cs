// -----------------------------------------------------------------------------
// <summary>
// A Date Type.
// </summary>
//
// <copyright file="DateType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Date data type in a DataFrame.
    /// </summary>
    public class DateType : IDataType
    {
        /// <summary>
        /// Gets the name of the Date type.
        /// </summary>
        /// <returns>The name of the Date type.</returns>
        public string TypeName() => nameof(DateType).Replace("Type", string.Empty);
    }
}
