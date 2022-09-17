﻿namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs before a request for ExistAsync in your repository pattern or query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBusinessBeforeExist<T, TKey>
        where TKey : notnull
    {
        Task<IStatedEntity<T, TKey>> BeforeExistAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
