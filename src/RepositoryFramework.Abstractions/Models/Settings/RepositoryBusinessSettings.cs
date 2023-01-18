using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    public sealed class RepositoryBusinessSettings<T, TKey>
        where TKey : notnull
    {
        public IServiceCollection Services { get; }
        public ServiceLifetime ServiceLifetime { get; }
        public RepositoryBusinessSettings(IServiceCollection services, ServiceLifetime? serviceLifetime = null)
        {
            Services = services;
            if (serviceLifetime != null)
                ServiceLifetime = serviceLifetime.Value;
            else
            {
                var entityType = typeof(T);
                var servicesByModel = RepositoryFrameworkRegistry.Instance.GetByModel(entityType);
                ServiceLifetime = servicesByModel.FirstOrDefault()?.ServiceLifetime ?? ServiceLifetime.Transient;
            }
        }
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeInsert<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeInsert<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Insert, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterInsert<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterInsert<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeUpdate<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterUpdate<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeDelete<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterDelete<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeBatch<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterBatch<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeGet<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterGet<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeExist<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeExist<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Exist, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterExist<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterExist<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Exist, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeQuery<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeQuery<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Query, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterQuery<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterQuery<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Query, true);
        public RepositoryBusinessSettings<T, TKey> AddBusinessBeforeOperation<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeOperation<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Operation, false);
        public RepositoryBusinessSettings<T, TKey> AddBusinessAfterOperation<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterOperation<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Operation, true);
        private RepositoryBusinessSettings<T, TKey> AddBusiness<TBusiness>(RepositoryMethods method, bool isAfterRequest)
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
    }
}
