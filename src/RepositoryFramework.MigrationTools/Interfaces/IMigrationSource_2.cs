﻿namespace RepositoryFramework.Migration
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods. Helper for Migration.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    public interface IMigrationSource<T, TKey> : IMigrationSource<T, TKey, bool>, IRepositoryPattern<T, TKey>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>
        where TKey : notnull
    {
    }
}