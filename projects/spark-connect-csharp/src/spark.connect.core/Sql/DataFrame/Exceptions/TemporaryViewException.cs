// -----------------------------------------------------------------------------
// <summary>
// An exception thrown when using Temporary Views.
// </summary>
//
// <copyright file="TemporaryViewException.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Exceptions
{
    /// <summary>
    /// Exception thrown when using Temporary Views.
    /// </summary>
    public class TemporaryViewException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryViewException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TemporaryViewException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryViewException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public TemporaryViewException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
