// -----------------------------------------------------------------------------
// <summary>
// An exception thrown when converting a query result to a DataFrame.
// </summary>
//
// <copyright file="DataFrameConversionException.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Exceptions
{
    /// <summary>
    /// Exception thrown when interacting with a Spark Session.
    /// </summary>
    public class DataFrameConversionException : Exception
    {
        /// <summary>
        /// The template message.
        /// </summary>
        public const string TemplateMessage = "Failed to convert to DataFrame: {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFrameConversionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DataFrameConversionException(string message)
            : base($"{TemplateMessage}: {message}") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFrameConversionException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public DataFrameConversionException(string message, Exception innerException)
            : base($"{TemplateMessage}: {message}", innerException) { }
    }
}
