﻿namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    public class TableStorageOptions<T, TKey>
        where TKey : notnull
    {
        internal static TableStorageOptions<T, TKey> Instance { get; } = new TableStorageOptions<T, TKey>();
        public Func<T, string> PartitionKeyFunction { get; internal set; } = null!;
        public Func<TKey, string> PartitionKeyFromKeyFunction { get; internal set; } = null!;
        public Func<T, string> RowKeyFunction { get; internal set; } = null!;
        public Func<TKey, string>? RowKeyFromKeyFunction { get; internal set; }
        public Func<T, DateTime>? TimestampFunction { get; internal set; }
        public string PartitionKey { get; internal set; } = null!;
        public string? RowKey { get; internal set; }
        public string? Timestamp { get; internal set; }
    }
}
