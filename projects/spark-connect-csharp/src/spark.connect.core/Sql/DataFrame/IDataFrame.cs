// -----------------------------------------------------------------------------
// <summary>
// A contract for Data Frames.
// </summary>
//
// <copyright file="IDataFrame.cs" company="Apache Software Foundation (ASF)">
// Copyright (c) Apache Software Foundation (ASF). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------------

using Spark.Connect.Core.Sql.DataFrame.Columns;
using Spark.Connect.Core.Sql.DataFrame.Exceptions;
using Spark.Connect.Core.Sql.DataFrame.Rows;
using Spark.Connect.Core.Sql.DataFrame.Types;
using Spark.Connect.Core.Sql.DataFrame.Writer;

namespace Spark.Connect.Core.Sql.DataFrame
{
    /// <summary>
    /// A wrapper for a data frame, representing a distributed collection of data rows.
    /// </summary>
    public interface IDataFrame
    {
        /// <summary>
        /// Prints out data frame data.
        /// </summary>
        /// <param name="numRows">The number of rows to show.</param>
        /// <param name="truncate">Whether to truncate long strings when printing.</param>
        /// <exception cref="DataFrameShowException">Thrown when an error occurs while showing the data frame.</exception>
        void Show(int numRows, bool truncate);

        /// <summary>
        /// Returns the schema for the current data frame.
        /// </summary>
        /// <returns>The schema for the current data frame.</returns>
        StructType Schema();

        /// <summary>
        /// Returns the data rows of the current data frame.
        /// </summary>
        /// <returns>The data rows of the current data frame.</returns>
        IRow[] Collect();

        /// <summary>
        /// Returns a data frame writer, which could be used to save data frame to supported storage.
        /// </summary>
        /// <returns>A data frame writer.</returns>
        IDataFrameWriter Write();

        /// <summary>
        /// Creates or replaces a temporary view.
        /// </summary>
        /// <param name="viewName">The name of the temporary view.</param>
        /// <param name="replace">Whether to replace the existing view with the same name.</param>
        /// <param name="global">Whether the view is global.</param>
        /// <exception cref="TemporaryViewException">Thrown when an error occurs creating or updating a temporary view.</exception>
        void CreateTempView(string viewName, bool replace, bool global);

        /// <summary>
        /// Re-partitions a data frame.
        /// </summary>
        /// <param name="numPartitions">The number of partitions.</param>
        /// <param name="columns">The columns to partition by.</param>
        /// <returns>The re-partitioned data frame.</returns>
        IDataFrame Repartition(int numPartitions, string[] columns);

        /// <summary>
        /// Re-partitions a data frame by range partition.
        /// </summary>
        /// <param name="numPartitions">The number of partitions.</param>
        /// <param name="columns">The range partition columns.</param>
        /// <returns>The re-partitioned data frame.</returns>
        IDataFrame RepartitionByRange(int numPartitions, RangePartitionColumn[] columns);
    }
}
