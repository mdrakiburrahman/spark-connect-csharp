// -----------------------------------------------------------------------------
// <summary>
// A Decimal Type.
// </summary>
//
// <copyright file="DecimalType.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Types
{
    /// <summary>
    /// Represents a Decimal data type in a DataFrame.
    /// </summary>
    public class DecimalType : IDataType
    {
        /// <summary>
        /// Gets the name of the Decimal type.
        /// </summary>
        /// <returns>The name of the Decimal type.</returns>
        public string TypeName() => nameof(DecimalType).Replace("Type", string.Empty);
    }
}
