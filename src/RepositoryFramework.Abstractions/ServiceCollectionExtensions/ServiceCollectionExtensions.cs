﻿using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static bool IsThatInterface<TEntity, TInterface>()
            => typeof(TEntity).GetInterfaces().Any(x => x == typeof(TInterface));
        private static Type? GetTypeOfModel<TEntity>()
            => GetType(typeof(TEntity), 0);
        private static Type? GetTypeOfKey<TEntity>()
            => GetType(typeof(TEntity), 1);
        private static Type? GetType(Type type, int index)
        {
            var service = type.GetInterfaces()
                .OrderByDescending(x => x.GetGenericArguments().Length)
                .FirstOrDefault(x => x.Name.Contains("IRepository") || x.Name.Contains("IQuery") || x.Name.Contains("ICommand"));
            if (service != null && type.GetGenericArguments().Length > index)
                return type.GetGenericArguments()[index];
            if (service != null && service.GetGenericArguments().Length > index)
                return service.GetGenericArguments()[index];
            else
            {
                Type? nextType = null;
                foreach (var interfaceType in type.GetInterfaces())
                {
                    nextType = GetType(interfaceType, index);
                    if (nextType != null)
                        return nextType;
                }
                if (type.BaseType != null && type.BaseType != typeof(object))
                    nextType = GetType(type.BaseType, index);
                if (nextType != null)
                    return nextType;
            }
            return null;
        }
        private static IServiceCollection AddServiceWithLifeTime<TInterface, TImplementation>(this IServiceCollection services,
          ServiceLifetime serviceLifetime)
            where TImplementation : class, TInterface
        {
            var entityType = GetTypeOfModel<TInterface>();
            if (entityType == null)
                throw new ArgumentNullException($"Model for {typeof(TInterface).FullName} not found. Check if your object {typeof(TInterface).Name} extends IRepository, IQuery or ICommand.");
            var keyType = GetTypeOfKey<TInterface>();
            if (keyType == null)
                throw new ArgumentNullException($"Key for {typeof(TInterface).FullName} not found. Check if your object {typeof(TInterface).Name} extends IRepository, IQuery or ICommand.");
            var service = RepositoryFrameworkServices.Instance.Services.FirstOrDefault(x => x.ModelType == entityType);
            if (service == null)
            {
                service = new RepositoryFrameworkService { ModelType = entityType };
                RepositoryFrameworkServices.Instance.Services.Add(service);
                services.AddSingleton(RepositoryFrameworkServices.Instance);
            }
            service.KeyType = keyType;

            if (IsThatInterface<TInterface, IRepositoryPattern>())
                service.RepositoryType = typeof(TInterface);
            else if (IsThatInterface<TInterface, ICommandPattern>())
                service.CommandType = typeof(TInterface);
            else if (IsThatInterface<TInterface, IQueryPattern>())
                service.QueryType = typeof(TInterface);

            return serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped(typeof(TInterface), typeof(TImplementation)),
                ServiceLifetime.Transient => services.AddTransient(typeof(TInterface), typeof(TImplementation)),
                ServiceLifetime.Singleton => services.AddSingleton(typeof(TInterface), typeof(TImplementation)),
                _ => services.AddScoped(typeof(TInterface), typeof(TImplementation))
            };
        }
        /// <summary>
        /// Add repository storage, inject the IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to retrieve, update or delete your data from repository</typeparam>
        /// <typeparam name="TStorage">Repository pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddRepository<T, TKey, TStorage>(this IServiceCollection services,
          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
          where TStorage : class, IRepository<T, TKey>
          where TKey : notnull
              => services.AddServiceWithLifeTime<IRepository<T, TKey>, TStorage>(serviceLifetime);
        /// <summary>
        /// Add Command storage for your CQRS, inject the ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// </summary>
        /// <typeparam name="T">Model used for your command</typeparam>
        /// <typeparam name="TKey">Key to store, update or delete your data</typeparam>
        /// <typeparam name="TStorage">Command pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddCommand<T, TKey, TStorage>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, ICommand<T, TKey>
            where TKey : notnull
              => services.AddServiceWithLifeTime<ICommand<T, TKey>, TStorage>(serviceLifetime);
        /// <summary>
        /// Add Query storage for your CQRS, inject the IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>>
        /// </summary>
        /// <typeparam name="T">Model used for your query</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TStorage">Query pattern storage</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="serviceLifetime">Service Lifetime</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddQuery<T, TKey, TStorage>(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TStorage : class, IQuery<T, TKey>
           where TKey : notnull
               => services.AddServiceWithLifeTime<IQuery<T, TKey>, TStorage>(serviceLifetime);
    }
}