using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBuilder<T, TKey>
          where TKey : notnull
    {
        IServiceCollection Services { get; }
        PatternType Type { get; }
        ServiceLifetime ServiceLifetime { get; }
        IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>();
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeInsert<TBusiness>()
            where TBusiness : class, IBusinessBeforeInsert<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterInsert<TBusiness>()
            where TBusiness : class, IBusinessAfterInsert<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeUpdate<TBusiness>()
            where TBusiness : class, IBusinessBeforeUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterUpdate<TBusiness>()
            where TBusiness : class, IBusinessAfterUpdate<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeDelete<TBusiness>()
            where TBusiness : class, IBusinessBeforeDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterDelete<TBusiness>()
            where TBusiness : class, IBusinessAfterDelete<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeBatch<TBusiness>()
            where TBusiness : class, IBusinessBeforeBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterBatch<TBusiness>()
            where TBusiness : class, IBusinessAfterBatch<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeGet<TBusiness>()
            where TBusiness : class, IBusinessBeforeGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterGet<TBusiness>()
            where TBusiness : class, IBusinessAfterGet<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeExist<TBusiness>()
           where TBusiness : class, IBusinessBeforeExist<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Exist, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterExist<TBusiness>()
            where TBusiness : class, IBusinessAfterExist<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Exist, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeQuery<TBusiness>()
           where TBusiness : class, IBusinessBeforeQuery<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Query, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterQuery<TBusiness>()
            where TBusiness : class, IBusinessAfterQuery<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Query, true);
        public IRepositoryBuilder<T, TKey> AddBusinessBeforeOperation<TBusiness>()
           where TBusiness : class, IBusinessBeforeOperation<T, TKey>
           => AddBusiness<TBusiness>(RepositoryMethods.Operation, false);
        public IRepositoryBuilder<T, TKey> AddBusinessAfterOperation<TBusiness>()
            where TBusiness : class, IBusinessAfterOperation<T, TKey>
            => AddBusiness<TBusiness>(RepositoryMethods.Operation, true);
        private IRepositoryBuilder<T, TKey> AddBusiness<TBusiness>(RepositoryMethods method, bool isPostRequest)
            where TBusiness : class
        {
            BusinessManagerOptions<T, TKey>.Instance.Services.Add(
               new BusinessType(method, typeof(TBusiness), isPostRequest));
            Services
                .AddService<IBusinessManager<T, TKey>, BusinessManager<T, TKey>>(ServiceLifetime)
                .AddSingleton(BusinessManagerOptions<T, TKey>.Instance)
                .AddService<TBusiness>(ServiceLifetime);
            return this;
        }
    }
}
