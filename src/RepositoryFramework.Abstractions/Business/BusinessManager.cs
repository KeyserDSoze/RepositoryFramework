using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    internal class BusinessManager<T, TKey> : IBusinessManager<T, TKey>
        where TKey : notnull
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceProvider ServiceProvider => _serviceProvider ??= _startingServiceProvider.CreateScope().ServiceProvider;
        private readonly IServiceProvider _startingServiceProvider;
        private readonly BusinessManagerOptions<T, TKey> _options;
        public BusinessManager(IServiceProvider serviceProvider, BusinessManagerOptions<T, TKey> options)
        {
            _startingServiceProvider = serviceProvider;
            _options = options;
        }
        private IEnumerable<BusinessType> PreInserted => GetBusinessTypes(RepositoryMethods.Insert, false);
        private IEnumerable<BusinessType> PostInserted => GetBusinessTypes(RepositoryMethods.Insert, true);
        private IEnumerable<BusinessType> PreUpdated => GetBusinessTypes(RepositoryMethods.Update, false);
        private IEnumerable<BusinessType> PostUpdated => GetBusinessTypes(RepositoryMethods.Update, true);
        private IEnumerable<BusinessType> PreDeleted => GetBusinessTypes(RepositoryMethods.Delete, false);
        private IEnumerable<BusinessType> PostDeleted => GetBusinessTypes(RepositoryMethods.Delete, true);
        private IEnumerable<BusinessType> PreBatched => GetBusinessTypes(RepositoryMethods.Batch, false);
        private IEnumerable<BusinessType> PostBatched => GetBusinessTypes(RepositoryMethods.Batch, true);
        private IEnumerable<BusinessType> PreGotten => GetBusinessTypes(RepositoryMethods.Get, false);
        private IEnumerable<BusinessType> PostGotten => GetBusinessTypes(RepositoryMethods.Get, true);
        private IEnumerable<BusinessType> PreExisted => GetBusinessTypes(RepositoryMethods.Exist, false);
        private IEnumerable<BusinessType> PostExisted => GetBusinessTypes(RepositoryMethods.Exist, true);
        private IEnumerable<BusinessType> PreQueried => GetBusinessTypes(RepositoryMethods.Query, false);
        private IEnumerable<BusinessType> PostQueried => GetBusinessTypes(RepositoryMethods.Query, true);
        private IEnumerable<BusinessType> PreOperation => GetBusinessTypes(RepositoryMethods.Operation, false);
        private IEnumerable<BusinessType> PostOperation => GetBusinessTypes(RepositoryMethods.Operation, true);
        public bool HasBusinessBeforeInsert => PreInserted.Any();
        public bool HasBusinessAfterInsert => PostInserted.Any();
        public bool HasBusinessBeforeUpdate => PreUpdated.Any();
        public bool HasBusinessAfterUpdate => PostUpdated.Any();
        public bool HasBusinessBeforeDelete => PreDeleted.Any();
        public bool HasBusinessAfterDelete => PostDeleted.Any();
        public bool HasBusinessBeforeBatch => PreBatched.Any();
        public bool HasBusinessAfterBatch => PostBatched.Any();
        public bool HasBusinessBeforeGet => PreGotten.Any();
        public bool HasBusinessAfterGet => PostGotten.Any();
        public bool HasBusinessBeforeExist => PreExisted.Any();
        public bool HasBusinessAfterExist => PostExisted.Any();
        public bool HasBusinessBeforeQuery => PreQueried.Any();
        public bool HasBusinessAfterQuery => PostQueried.Any();
        public bool HasBusinessBeforeOperation => PreOperation.Any();
        public bool HasBusinessAfterOperation => PostOperation.Any();
        private IEnumerable<BusinessType> GetBusinessTypes(RepositoryMethods method, bool isPostRequest)
            => _options.Services.Where(x => x.Method == method && x.IsPostRequest == isPostRequest);
        private IEnumerable<TBusiness> GetBusinesses<TBusiness>(IEnumerable<BusinessType> business)
            => business.Select(x => (TBusiness)ServiceProvider.GetService(x.Service)!);
        public async Task<IState<T>> InsertAsync(ICommandPattern<T, TKey> command, TKey key, T value, CancellationToken cancellationToken = default)
        {
            var entity = IEntity.Default(key, value);

            foreach (var preInserted in GetBusinesses<IBusinessBeforeInsert<T, TKey>>(PreInserted))
                entity = await preInserted.InsertAsync(entity.Key, entity.Value, cancellationToken);

            var response = await command.InsertAsync(entity.Key, entity.Value, cancellationToken);

            foreach (var postInserted in GetBusinesses<IBusinessAfterInsert<T, TKey>>(PostInserted))
                response = await postInserted.InsertAsync(response, entity.Key, entity.Value, cancellationToken);

            return response;
        }
        public async Task<IState<T>> UpdateAsync(ICommandPattern<T, TKey> command, TKey key, T value, CancellationToken cancellationToken = default)
        {
            var entity = IEntity.Default(key, value);

            foreach (var preUpdated in GetBusinesses<IBusinessBeforeUpdate<T, TKey>>(PreUpdated))
                entity = await preUpdated.UpdateAsync(entity.Key, entity.Value, cancellationToken);

            var response = await command.UpdateAsync(entity.Key, entity.Value, cancellationToken);

            foreach (var postUpdated in GetBusinesses<IBusinessAfterUpdate<T, TKey>>(PostUpdated))
                response = await postUpdated.UpdateAsync(response, entity.Key, entity.Value, cancellationToken);

            return response;
        }
        public async Task<IState<T>> DeleteAsync(ICommandPattern<T, TKey> command, TKey key, CancellationToken cancellationToken = default)
        {
            foreach (var preDeleted in GetBusinesses<IBusinessBeforeDelete<T, TKey>>(PreDeleted))
                key = await preDeleted.DeleteAsync(key, cancellationToken);

            var response = await command.DeleteAsync(key, cancellationToken);

            foreach (var postDeleted in GetBusinesses<IBusinessAfterDelete<T, TKey>>(PostDeleted))
                response = await postDeleted.DeleteAsync(response, key, cancellationToken);

            return response;
        }

        public async Task<BatchResults<T, TKey>> BatchAsync(ICommandPattern<T, TKey> command, BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            foreach (var preBatch in GetBusinesses<IBusinessBeforeBatch<T, TKey>>(PreBatched))
                operations = await preBatch.BatchAsync(operations, cancellationToken);

            var response = await command.BatchAsync(operations, cancellationToken);

            foreach (var postBatch in GetBusinesses<IBusinessAfterBatch<T, TKey>>(PostBatched))
                response = await postBatch.BatchAsync(response, operations, cancellationToken);

            return response;
        }

        public async Task<IState<T>> ExistAsync(IQueryPattern<T, TKey> query, TKey key, CancellationToken cancellationToken = default)
        {
            foreach (var preExisted in GetBusinesses<IBusinessBeforeExist<T, TKey>>(PreExisted))
                key = await preExisted.ExistAsync(key, cancellationToken);

            var response = await query.ExistAsync(key, cancellationToken);

            foreach (var postExisted in GetBusinesses<IBusinessAfterExist<T, TKey>>(PostExisted))
                response = await postExisted.ExistAsync(response, key, cancellationToken);

            return response;
        }

        public async Task<T?> GetAsync(IQueryPattern<T, TKey> query, TKey key, CancellationToken cancellationToken = default)
        {
            foreach (var preGotten in GetBusinesses<IBusinessBeforeGet<T, TKey>>(PreGotten))
                key = await preGotten.GetAsync(key, cancellationToken);

            var response = await query.GetAsync(key, cancellationToken);

            foreach (var postGotten in GetBusinesses<IBusinessAfterGet<T, TKey>>(PostGotten))
                response = await postGotten.GetAsync(response, key, cancellationToken);

            return response;
        }

        public async IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IQueryPattern<T, TKey> queryPattern, Query query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var preQueried in GetBusinesses<IBusinessBeforeQuery<T, TKey>>(PreQueried))
                query = await preQueried.QueryAsync(query, cancellationToken);

            await foreach (var entity in queryPattern.QueryAsync(query, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = entity;
                foreach (var postGotten in GetBusinesses<IBusinessAfterQuery<T, TKey>>(PostQueried))
                    result = await postGotten.QueryAsync(entity, query, cancellationToken);
                yield return result;
            }
        }

        public async ValueTask<TProperty> OperationAsync<TProperty>(IQueryPattern<T, TKey> queryPattern, OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default)
        {
            (OperationType<TProperty> Operation, Query Query) operationQuery = (operation, query);

            foreach (var preOperation in GetBusinesses<IBusinessBeforeOperation<T, TKey>>(PreOperation))
                operationQuery = await preOperation.OperationAsync(operationQuery.Operation, operationQuery.Query, cancellationToken);

            var response = await queryPattern.OperationAsync(operationQuery.Operation, operationQuery.Query, cancellationToken);

            foreach (var postOperation in GetBusinesses<IBusinessAfterOperation<T, TKey>>(PostOperation))
                response = await postOperation.OperationAsync(response, operationQuery.Operation, operationQuery.Query, cancellationToken);

            return response;
        }
    }
}
