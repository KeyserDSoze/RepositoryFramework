﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RepositoryFramework
{
    internal sealed class RepositorySettings<T, TKey> : IRepositorySettings<T, TKey>
        where TKey : notnull
    {
        public IServiceCollection Services { get; }
        public void SetNotExposable()
        {
            var service = SetService();
            service.IsNotExposable = false;
        }
        public RepositorySettings(IServiceCollection services)
        {
            Services = services;
        }
        public IRepositoryBuilder<T, TKey, TStorage> SetStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IRepository<T, TKey>
        {
            var service = SetService();
            var currentType = typeof(IRepositoryPattern<T, TKey>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            Services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(IRepositoryPattern<T, TKey>))
                .AddService<IRepositoryPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<IRepository<T, TKey>, Repository<T, TKey>>(serviceLifetime);
            return new RepositoryBuilder<T, TKey, TStorage>(Services, PatternType.Repository, serviceLifetime);
        }
        public IRepositoryBuilder<T, TKey, TStorage> SetCommandStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ICommand<T, TKey>
        {
            var service = SetService();
            var currentType = typeof(ICommandPattern<T, TKey>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            Services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(ICommandPattern<T, TKey>))
                .AddService<ICommandPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<ICommand<T, TKey>, Command<T, TKey>>(serviceLifetime);
            return new RepositoryBuilder<T, TKey, TStorage>(Services, PatternType.Command, serviceLifetime);
        }
        public IRepositoryBuilder<T, TKey, TStorage> SetQueryStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IQuery<T, TKey>
        {
            var service = SetService();
            var currentType = typeof(IQueryPattern<T, TKey>);
            service.AddOrUpdate(currentType, typeof(TStorage));
            Services
                .RemoveServiceIfAlreadyInstalled<TStorage>(currentType, typeof(IQueryPattern<T, TKey>))
                .AddService<IQueryPattern<T, TKey>, TStorage>(serviceLifetime)
                .AddService<IQuery<T, TKey>, Query<T, TKey>>(serviceLifetime);
            return new RepositoryBuilder<T, TKey, TStorage>(Services, PatternType.Query, serviceLifetime);
        }
        private RepositoryFrameworkService SetService()
        {
            var entityType = typeof(T);
            var keyType = typeof(TKey);
            var service = RepositoryFrameworkRegistry.Instance.Services.FirstOrDefault(x => x.ModelType == entityType);
            if (service == null)
            {
                service = new RepositoryFrameworkService(keyType, entityType);
                RepositoryFrameworkRegistry.Instance.Services.Add(service);
                Services.TryAddSingleton(RepositoryFrameworkRegistry.Instance);
            }
            return service;
        }
    }
}
