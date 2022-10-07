using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    internal sealed class DataverseRepository<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly ServiceClient _client;
        private readonly PropertyInfo[] _properties;
        private readonly DataverseOptions<T, TKey> _settings;
        private readonly IDataverseKeyManager<T, TKey> _keyManager;

        public DataverseRepository(DataverseOptions<T, TKey> settings,
            IDataverseKeyManager<T, TKey> keyManager)
        {
            _client = settings.GetClient();
            _settings = settings;
            _keyManager = keyManager;
        }
        public async Task<State<T, TKey>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<State<T, TKey>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<State<T, TKey>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<Entity<T, TKey>> QueryAsync(IFilterExpression filter,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression filter,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<State<T, TKey>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
