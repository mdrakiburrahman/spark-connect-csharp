// -----------------------------------------------------------------------------
// <summary>
// A data frame writer.
// </summary>
//
// <copyright file="DataFrameWriter.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

namespace Spark.Connect.Core.Sql.DataFrame.Writer
{
    /// <summary>
    /// DataFrameWriter supports writing data frame to storage.
    /// </summary>
    public class DataFrameWriter : IDataFrameWriter
    {
        /// <inheritdoc/>
        public IDataFrameWriter Format(string source)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IDataFrameWriter Mode(string saveMode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Save(string path)
        {
            throw new NotImplementedException();
        }
    }
}
