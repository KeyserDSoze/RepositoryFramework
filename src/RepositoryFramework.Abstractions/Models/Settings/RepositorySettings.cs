using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RepositoryFramework
{
    public sealed class RepositorySettings<T, TKey>
        where TKey : notnull
    {
        public IServiceCollection Services { get; }
        public PatternType Type { get; private set; }
        public ServiceLifetime ServiceLifetime { get; private set; }
        public void SetNotExposable()
        {
            var service = SetService();
            service.IsNotExposable = false;
        }
        public RepositorySettings(IServiceCollection services)
        {
            Services = services;
        }
        public IRepositoryBuilder<T, TKey, TStorage> SetStorage<TStorage>(PatternType type, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IRepository<T, TKey>
            => type switch
            {
                PatternType.Command => SetCommandStorage<TStorage>(serviceLifetime),
                PatternType.Query => SetQueryStorage<TStorage>(serviceLifetime),
                _ => SetRepositoryStorage<TStorage>(serviceLifetime)
            };
        public IRepositoryBuilder<T, TKey, TStorage> SetRepositoryStorage<TStorage>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStorage : class, IRepository<T, TKey>
        {
            var service = SetService();
            Type = PatternType.Repository;
            ServiceLifetime = serviceLifetime;
            var currentType = typeof(IRepository<T, TKey>);
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
            Type = PatternType.Command;
            ServiceLifetime = serviceLifetime;
            var currentType = typeof(ICommand<T, TKey>);
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
            var currentType = typeof(IQuery<T, TKey>);
            Type = PatternType.Query;
            ServiceLifetime = serviceLifetime;
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
        public RepositorySettings<T, TKey> AddBusinessBeforeInsert<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeInsert<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Insert, false);
        public RepositorySettings<T, TKey> AddBusinessAfterInsert<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterInsert<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeUpdate<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, false);
        public RepositorySettings<T, TKey> AddBusinessAfterUpdate<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeDelete<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, false);
        public RepositorySettings<T, TKey> AddBusinessAfterDelete<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeBatch<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, false);
        public RepositorySettings<T, TKey> AddBusinessAfterBatch<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeGet<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, false);
        public RepositorySettings<T, TKey> AddBusinessAfterGet<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeExist<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeExist<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Exist, false);
        public RepositorySettings<T, TKey> AddBusinessAfterExist<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterExist<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Exist, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeQuery<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeQuery<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Query, false);
        public RepositorySettings<T, TKey> AddBusinessAfterQuery<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterQuery<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Query, true);
        public RepositorySettings<T, TKey> AddBusinessBeforeOperation<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeOperation<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Operation, false);
        public RepositorySettings<T, TKey> AddBusinessAfterOperation<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterOperation<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Operation, true);
        private RepositorySettings<T, TKey> AddBusiness<TBusiness>(RepositoryMethods method, bool isAfterRequest)
            where TBusiness : class
        {
            BusinessManagerOptions<T, TKey>.Instance.Services.Add(
               new BusinessType(method, typeof(TBusiness), isAfterRequest));
            Services
                .AddService<IRepositoryBusinessManager<T, TKey>, RepositoryBusinessManager<T, TKey>>(ServiceLifetime)
                .AddSingleton(BusinessManagerOptions<T, TKey>.Instance)
                .AddService<TBusiness>(ServiceLifetime);
            return this;
        }
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
        {
            Services.AddSingleton<IRepositoryFilterTranslator<T, TKey>>(FilterTranslation<T, TKey>.Instance);
            FilterTranslation<T, TKey>.Instance.Setup<TTranslated>();
            Services.AddSingleton<IRepositoryMapper<T, TKey, TTranslated>>(RepositoryMapper<T, TKey, TTranslated>.Instance);
            return new QueryTranslationBuilder<T, TKey, TTranslated>(this);
        }
    }
}
