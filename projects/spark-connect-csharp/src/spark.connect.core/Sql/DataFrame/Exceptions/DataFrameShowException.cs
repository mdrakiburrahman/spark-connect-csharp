// -----------------------------------------------------------------------------
// <summary>
// An exception thrown when an error occurs while showing a DataFrame.
// </summary>
//
// <copyright file="DataFrameShowException.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs while showing a DataFrame.
    /// </summary>
    public class DataFrameShowException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataFrameShowException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DataFrameShowException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFrameShowException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public DataFrameShowException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
