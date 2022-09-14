namespace RepositoryFramework.WebApi
{
    public class IperUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class IperRepositoryStorage : IRepository<IperUser>
    {
        public Task<BatchResults<IperUser, string>> BatchAsync(BatchOperations<IperUser, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<IperUser>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new ArgumentException("dasdsada");
            //throw new NotImplementedException("Compit");
        }

        public Task<State<IperUser>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Saalsbury");
        }

        public Task<IperUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new IperUser { Id = "2", Name = "d"});
        }

        public Task<State<IperUser>> InsertAsync(string key, IperUser value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEntity<IperUser, string>> QueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<State<IperUser>> UpdateAsync(string key, IperUser value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0);
            return true;
        }
    }
}
