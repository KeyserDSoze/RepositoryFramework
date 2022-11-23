﻿using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder with State as default TState.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    internal class RepositoryBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        public IServiceCollection Services { get; }
        public PatternType Type { get; }
        public ServiceLifetime ServiceLifetime { get; }
        public RepositoryBuilder(IServiceCollection services, PatternType type, ServiceLifetime serviceLifetime)
        {
            Services = services;
            Type = type;
            ServiceLifetime = serviceLifetime;
        }
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
        {
            Services.AddSingleton<IRepositoryFilterTranslator<T, TKey>>(FilterTranslation<T, TKey>.Instance);
            Services.AddSingleton<IRepositoryMapper<T, TKey, TTranslated>>(RepositoryMapper<T, TKey, TTranslated>.Instance);
            return new QueryTranslationBuilder<T, TKey, TTranslated>(this);
        }
    }
}
