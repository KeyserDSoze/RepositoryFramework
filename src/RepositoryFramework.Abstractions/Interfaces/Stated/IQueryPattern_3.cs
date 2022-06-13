﻿using System.Linq.Expressions;

namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query and Exist methods.
    /// This is the interface that you need to extend if you want to create your query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to retrieve your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface IQueryPattern<T, TKey, TState> : IQueryPattern
        where TKey : notnull
    {
        Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default);
    }
}