// -----------------------------------------------------------------------------
// <summary>
// An exception thrown when no data is received from the Server.
// </summary>
//
// <copyright file="NoDataReceivedException.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Exceptions
{
    /// <summary>
    /// Exception thrown when no data is received for a query.
    /// </summary>
    public class NoDataReceivedException : Exception
    {
        /// <summary>
        /// The template message.
        /// </summary>
        public const string TemplateMessage = "No data received from server: {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataReceivedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NoDataReceivedException(string message)
            : base($"{TemplateMessage}: {message}") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataReceivedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public NoDataReceivedException(string message, Exception innerException)
            : base($"{TemplateMessage}: {message}", innerException) { }
    }
}
