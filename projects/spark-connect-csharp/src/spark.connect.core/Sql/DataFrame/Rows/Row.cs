// -----------------------------------------------------------------------------
// <summary>
// A Data Frame row.
// </summary>
//
// <copyright file="Row.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.DataFrame.Types;

namespace Spark.Connect.Core.Sql.DataFrame.Rows
{
    /// <summary>
    /// A Data Frame row.
    /// </summary>
    public class Row : IRow
    {
        private readonly object[] values;
        private readonly StructType schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        /// <param name="values">The values of the row.</param>
        /// <param name="schema">The schema of the row.</param>
        public Row(object[] values, StructType schema)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
            this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        /// <summary>
        /// Gets the schema of the row.
        /// </summary>
        /// <returns>The schema of the row.</returns>
        public StructType Schema() => this.schema;

        /// <summary>
        /// Gets the values of the row.
        /// </summary>
        /// <returns>The values of the row.</returns>
        public object[] Values() => this.values;
    }
}
