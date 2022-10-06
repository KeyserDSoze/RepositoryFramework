using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    internal class RepositoryBusinessManager<T, TKey> : IRepositoryBusinessManager<T, TKey>
        where TKey : notnull
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceProvider ServiceProvider => _serviceProvider ??= _startingServiceProvider.CreateScope().ServiceProvider;
        private readonly IServiceProvider _startingServiceProvider;
        private readonly BusinessManagerOptions<T, TKey> _options;
        public RepositoryBusinessManager(IServiceProvider serviceProvider, BusinessManagerOptions<T, TKey> options)
        {
            _startingServiceProvider = serviceProvider;
            _options = options;
        }
        private IEnumerable<BusinessType> BeforeInserted => GetBusinessTypes(RepositoryMethods.Insert, false);
        private IEnumerable<BusinessType> AfterInserted => GetBusinessTypes(RepositoryMethods.Insert, true);
        private IEnumerable<BusinessType> BeforeUpdated => GetBusinessTypes(RepositoryMethods.Update, false);
        private IEnumerable<BusinessType> AfterUpdated => GetBusinessTypes(RepositoryMethods.Update, true);
        private IEnumerable<BusinessType> BeforeDeleted => GetBusinessTypes(RepositoryMethods.Delete, false);
        private IEnumerable<BusinessType> AfterDeleted => GetBusinessTypes(RepositoryMethods.Delete, true);
        private IEnumerable<BusinessType> BeforeBatched => GetBusinessTypes(RepositoryMethods.Batch, false);
        private IEnumerable<BusinessType> AfterBatched => GetBusinessTypes(RepositoryMethods.Batch, true);
        private IEnumerable<BusinessType> BeforeGotten => GetBusinessTypes(RepositoryMethods.Get, false);
        private IEnumerable<BusinessType> AfterGotten => GetBusinessTypes(RepositoryMethods.Get, true);
        private IEnumerable<BusinessType> BeforeExisted => GetBusinessTypes(RepositoryMethods.Exist, false);
        private IEnumerable<BusinessType> AfterExisted => GetBusinessTypes(RepositoryMethods.Exist, true);
        private IEnumerable<BusinessType> BeforeQueried => GetBusinessTypes(RepositoryMethods.Query, false);
        private IEnumerable<BusinessType> AfterQueried => GetBusinessTypes(RepositoryMethods.Query, true);
        private IEnumerable<BusinessType> BeforeOperation => GetBusinessTypes(RepositoryMethods.Operation, false);
        private IEnumerable<BusinessType> AfterOperation => GetBusinessTypes(RepositoryMethods.Operation, true);
        public bool HasBusinessBeforeInsert => BeforeInserted.Any();
        public bool HasBusinessAfterInsert => AfterInserted.Any();
        public bool HasBusinessBeforeUpdate => BeforeUpdated.Any();
        public bool HasBusinessAfterUpdate => AfterUpdated.Any();
        public bool HasBusinessBeforeDelete => BeforeDeleted.Any();
        public bool HasBusinessAfterDelete => AfterDeleted.Any();
        public bool HasBusinessBeforeBatch => BeforeBatched.Any();
        public bool HasBusinessAfterBatch => AfterBatched.Any();
        public bool HasBusinessBeforeGet => BeforeGotten.Any();
        public bool HasBusinessAfterGet => AfterGotten.Any();
        public bool HasBusinessBeforeExist => BeforeExisted.Any();
        public bool HasBusinessAfterExist => AfterExisted.Any();
        public bool HasBusinessBeforeQuery => BeforeQueried.Any();
        public bool HasBusinessAfterQuery => AfterQueried.Any();
        public bool HasBusinessBeforeOperation => BeforeOperation.Any();
        public bool HasBusinessAfterOperation => AfterOperation.Any();
        private IEnumerable<BusinessType> GetBusinessTypes(RepositoryMethods method, bool isAfterRequest)
            => _options.Services.Where(x => x.Method == method && x.IsAfterRequest == isAfterRequest);
        private IEnumerable<TBusiness> GetBusiness<TBusiness>(IEnumerable<BusinessType> business)
            => business.Select(x => (TBusiness)ServiceProvider.GetService(x.Service)!);
        public async Task<State<T, TKey>> InsertAsync(ICommandPattern<T, TKey> command, TKey key, T value, CancellationToken cancellationToken = default)
        {
            var entity = Entity.Default(value, key);
            var result = entity.ToOkState();

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeInsert<T, TKey>>(BeforeInserted))
            {
                result = await business.BeforeInsertAsync(result.Entity!, cancellationToken);
                if (!result.HasEntity)
                    result.Entity = entity;
                if (!result.IsOk)
                    return result;
            }

            result = await command.InsertAsync(result.Entity!.Key!, result.Entity!.Value!, cancellationToken);
            entity = result.Entity;

            foreach (var business in GetBusiness<IRepositoryBusinessAfterInsert<T, TKey>>(AfterInserted))
            {
                result = await business.AfterInsertAsync(result, result.Entity!, cancellationToken);
                if (!result.HasEntity)
                    result.Entity = entity;
            }

            return result;
        }
        public async Task<State<T, TKey>> UpdateAsync(ICommandPattern<T, TKey> command, TKey key, T value, CancellationToken cancellationToken = default)
        {
            var entity = Entity.Default(value, key);
            var result = entity.ToOkState();

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeUpdate<T, TKey>>(BeforeUpdated))
            {
                result = await business.BeforeUpdateAsync(result.Entity!, cancellationToken);
                if (!result.HasEntity)
                    result.Entity = entity;
                if (!result.IsOk)
                    return result;
            }

            result = await command.UpdateAsync(result.Entity!.Key!, result.Entity!.Value!, cancellationToken);
            entity = result.Entity!;

            foreach (var business in GetBusiness<IRepositoryBusinessAfterUpdate<T, TKey>>(AfterUpdated))
            {
                result = await business.AfterUpdateAsync(result, result.Entity!, cancellationToken);
                if (!result.HasEntity)
                    result.Entity = entity;
            }

            return result;
        }
        public async Task<State<T, TKey>> DeleteAsync(ICommandPattern<T, TKey> command, TKey key, CancellationToken cancellationToken = default)
        {
            var entity = Entity.Default(default(T)!, key);
            var result = entity.ToOkState();

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeDelete<T, TKey>>(BeforeDeleted))
            {
                result = await business.BeforeDeleteAsync(result.Entity!.Key!, cancellationToken);
                if (!result.IsOk)
                    return result;
            }

            result = await command.DeleteAsync(result.Entity!.Key!, cancellationToken);

            foreach (var business in GetBusiness<IRepositoryBusinessAfterDelete<T, TKey>>(AfterDeleted))
                result = await business.AfterDeleteAsync(result, result.Entity!.Key!, cancellationToken);

            return result;
        }

        public async Task<BatchResults<T, TKey>> BatchAsync(ICommandPattern<T, TKey> command, BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            var results = BatchResults<T, TKey>.Empty;

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeBatch<T, TKey>>(BeforeBatched))
            {
                results = await business.BeforeBatchAsync(operations, cancellationToken);
                foreach (var result in results.Results)
                    if (!result.State.IsOk)
                    {
                        results.Results.Add(result);
                        operations.Values.Remove(operations.Values.First(x => x.Key.Equals(result.Key)));
                    }
            }

            if (operations.Values.Count > 0)
            {
                var response = await command.BatchAsync(operations, cancellationToken);
                results.Results.AddRange(response.Results);
            }

            foreach (var business in GetBusiness<IRepositoryBusinessAfterBatch<T, TKey>>(AfterBatched))
                results = await business.AfterBatchAsync(results, operations, cancellationToken);

            return results;
        }

        public async Task<State<T, TKey>> ExistAsync(IQueryPattern<T, TKey> query, TKey key, CancellationToken cancellationToken = default)
        {
            var entity = Entity.Default(default(T)!, key);
            var result = entity.ToOkState();

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeExist<T, TKey>>(BeforeExisted))
            {
                result = await business.BeforeExistAsync(result.Entity!.Key!, cancellationToken);
                if (!result.IsOk)
                    return result;
            }

            var response = await query.ExistAsync(result.Entity!.Key!, cancellationToken);

            foreach (var business in GetBusiness<IRepositoryBusinessAfterExist<T, TKey>>(AfterExisted))
                response = await business.AfterExistAsync(response, result.Entity!.Key!, cancellationToken);

            return response;
        }

        public async Task<T?> GetAsync(IQueryPattern<T, TKey> query, TKey key, CancellationToken cancellationToken = default)
        {
            var entity = Entity.Default(default(T)!, key);
            var result = entity.ToOkState();

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeGet<T, TKey>>(BeforeGotten))
            {
                result = await business.BeforeGetAsync(result.Entity!.Key!, cancellationToken);
                if (result.HasEntity && result.Entity!.HasValue)
                    return result.Entity.Value;
                else if (!result.IsOk)
                    return default;
            }

            var response = await query.GetAsync(result.Entity!.Key!, cancellationToken);

            foreach (var business in GetBusiness<IRepositoryBusinessAfterGet<T, TKey>>(AfterGotten))
                response = await business.AfterGetAsync(response, result.Entity!.Key!, cancellationToken);

            return response;
        }

        public async IAsyncEnumerable<Entity<T, TKey>> QueryAsync(IQueryPattern<T, TKey> queryPattern, IFilterExpression filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var business in GetBusiness<IRepositoryBusinessBeforeQuery<T, TKey>>(BeforeQueried))
                filter = await business.BeforeQueryAsync(filter, cancellationToken);

            if (HasBusinessAfterQuery)
            {
                var items = await queryPattern.QueryAsync(filter, cancellationToken).ToListAsync().NoContext();
                foreach (var business in GetBusiness<IRepositoryBusinessAfterQuery<T, TKey>>(AfterQueried))
                    items = await business.AfterQueryAsync(items, filter, cancellationToken);

                foreach (var item in items)
                    yield return item;
            }
            else
            {
                await foreach (var item in queryPattern.QueryAsync(filter, cancellationToken))
                    yield return item;
            }
        }

        public async ValueTask<TProperty> OperationAsync<TProperty>(IQueryPattern<T, TKey> queryPattern, OperationType<TProperty> operation, IFilterExpression filter, CancellationToken cancellationToken = default)
        {
            (OperationType<TProperty> Operation, IFilterExpression Filter) operationFilter = (operation, filter);

            foreach (var business in GetBusiness<IRepositoryBusinessBeforeOperation<T, TKey>>(BeforeOperation))
                operationFilter = await business.BeforeOperationAsync(operationFilter.Operation, operationFilter.Filter, cancellationToken);

            var response = await queryPattern.OperationAsync(operationFilter.Operation, operationFilter.Filter, cancellationToken);

            foreach (var business in GetBusiness<IRepositoryBusinessAfterOperation<T, TKey>>(AfterOperation))
                response = await business.AfterOperationAsync(response, operationFilter.Operation, operationFilter.Filter, cancellationToken);

            return response;
        }
    }
}
