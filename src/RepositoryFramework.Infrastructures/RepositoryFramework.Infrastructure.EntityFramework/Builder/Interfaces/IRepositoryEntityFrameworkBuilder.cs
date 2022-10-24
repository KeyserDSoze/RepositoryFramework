﻿using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.EntityFramework
{
    public interface IRepositoryEntityFrameworkBuilder<T, TKey, TEntityModel> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
        where TEntityModel : class
    {
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}