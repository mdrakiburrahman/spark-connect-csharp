// -----------------------------------------------------------------------------
// <summary>
// An exception thrown when executing a query.
// </summary>
//
// <copyright file="QueryExecutionException.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Exceptions
{
    /// <summary>
    /// Exception thrown when executing a query.
    /// </summary>
    public class QueryExecutionException : Exception
    {
        /// <summary>
        /// The template message.
        /// </summary>
        public const string TemplateMessage = "Failed to execute query: {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExecutionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public QueryExecutionException(string message)
            : base($"{TemplateMessage}: {message}") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExecutionException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public QueryExecutionException(string message, Exception innerException)
            : base($"{TemplateMessage}: {message}", innerException) { }
    }
}
