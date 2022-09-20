﻿namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs after a request for DeleteAsync in your repository pattern or command pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBusinessAfterDelete<T, TKey>
      where TKey : notnull
    {
        Task<IState<T>> AfterDeleteAsync(IState<T> state, TKey key, CancellationToken cancellationToken = default);
    }
}