using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface IRepositoryBuilder<T> : IRepositoryBuilder<T, string>
    {
        new IQueryTranslationBuilder<T, TTranslated> Translate<TTranslated>();
        public new IRepositoryBuilder<T> AddBusinessBeforeInsert<TBusiness>()
           where TBusiness : class, IBusinessBeforeInsert<T>
           => AddBusiness<TBusiness>(RepositoryMethods.Insert, false);
        public new IRepositoryBuilder<T> AddBusinessAfterInsert<TBusiness>()
            where TBusiness : class, IBusinessAfterInsert<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Insert, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeUpdate<TBusiness>()
            where TBusiness : class, IBusinessBeforeUpdate<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, false);
        public new IRepositoryBuilder<T> AddBusinessAfterUpdate<TBusiness>()
            where TBusiness : class, IBusinessAfterUpdate<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Update, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeDelete<TBusiness>()
            where TBusiness : class, IBusinessBeforeDelete<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, false);
        public new IRepositoryBuilder<T> AddBusinessAfterDelete<TBusiness>()
            where TBusiness : class, IBusinessAfterDelete<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Delete, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeBatch<TBusiness>()
            where TBusiness : class, IBusinessBeforeBatch<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, false);
        public new IRepositoryBuilder<T> AddBusinessAfterBatch<TBusiness>()
            where TBusiness : class, IBusinessAfterBatch<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Batch, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeGet<TBusiness>()
            where TBusiness : class, IBusinessBeforeGet<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, false);
        public new IRepositoryBuilder<T> AddBusinessAfterGet<TBusiness>()
            where TBusiness : class, IBusinessAfterGet<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Get, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeExist<TBusiness>()
           where TBusiness : class, IBusinessBeforeExist<T>
           => AddBusiness<TBusiness>(RepositoryMethods.Exist, false);
        public new IRepositoryBuilder<T> AddBusinessAfterExist<TBusiness>()
            where TBusiness : class, IBusinessAfterExist<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Exist, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeQuery<TBusiness>()
           where TBusiness : class, IBusinessBeforeQuery<T>
           => AddBusiness<TBusiness>(RepositoryMethods.Query, false);
        public new IRepositoryBuilder<T> AddBusinessAfterQuery<TBusiness>()
            where TBusiness : class, IBusinessAfterQuery<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Query, true);
        public new IRepositoryBuilder<T> AddBusinessBeforeOperation<TBusiness>()
           where TBusiness : class, IBusinessBeforeOperation<T>
           => AddBusiness<TBusiness>(RepositoryMethods.Operation, false);
        public new IRepositoryBuilder<T> AddBusinessAfterOperation<TBusiness>()
            where TBusiness : class, IBusinessAfterOperation<T>
            => AddBusiness<TBusiness>(RepositoryMethods.Operation, true);
        private IRepositoryBuilder<T> AddBusiness<TBusiness>(RepositoryMethods method, bool isPostRequest)
            where TBusiness : class
        {
            BusinessManagerOptions<T, string>.Instance.Services.Add(
               new BusinessType(method, typeof(TBusiness), isPostRequest));
            Services
                .AddService<IBusinessManager<T>, BusinessManager<T>>(ServiceLifetime)
                .AddSingleton(BusinessManagerOptions<T, string>.Instance)
                .AddService<TBusiness>(ServiceLifetime);
            return this;
        }
    }
}
