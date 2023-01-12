using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder for your repository framewrk
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TStorage">Storage for your repository.</typeparam>
    public interface IRepositoryBuilder<T, TKey, TStorage>
          where TKey : notnull
        where TStorage : class
    {
        IServiceCollection Services { get; }
        PatternType Type { get; }
        ServiceLifetime ServiceLifetime { get; }
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeInsert<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeInsert<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterInsert<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterInsert<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeUpdate<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterUpdate<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeDelete<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterDelete<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeBatch<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterBatch<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeGet<TBusiness>()
            where TBusiness : class, IRepositoryBusinessBeforeGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterGet<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeExist<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeExist<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Exist, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterExist<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterExist<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Exist, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeQuery<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeQuery<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Query, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterQuery<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterQuery<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Query, true);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessBeforeOperation<TBusiness>()
           where TBusiness : class, IRepositoryBusinessBeforeOperation<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Operation, false);
        public IRepositoryBuilder<T, TKey, TStorage> AddBusinessAfterOperation<TBusiness>()
            where TBusiness : class, IRepositoryBusinessAfterOperation<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Operation, true);
        private IRepositoryBuilder<T, TKey, TStorage> AddBusiness<TBusiness>(RepositoryMethods method, bool isAfterRequest)
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
        public IQueryTranslationBuilder<T, TKey, TStorage, TTranslated> Translate<TTranslated>()
        {
            Services.AddSingleton<IRepositoryFilterTranslator<T, TKey>>(FilterTranslation<T, TKey>.Instance);
            FilterTranslation<T, TKey>.Instance.Setup<TTranslated>();
            Services.AddSingleton<IRepositoryMapper<T, TKey, TTranslated>>(RepositoryMapper<T, TKey, TTranslated>.Instance);
            return new QueryTranslationBuilder<T, TKey, TStorage, TTranslated>(this);
        }
    }
}
