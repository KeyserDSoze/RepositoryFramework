using System.Runtime.CompilerServices;

namespace RepositoryFramework.WebApi
{
    public class IperUser
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Email { get; } = null!;
        public int Port { get; set; }
        public bool IsAdmin { get; set; }
        public Guid GroupId { get; set; }
    }
    public class IperRepositoryBeforeInsertBusiness : IRepositoryBusinessBeforeInsert<IperUser, string>
    {
        public Task<IStatedEntity<IperUser, string>> BeforeInsertAsync(IEntity<IperUser, string> entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(IStatedEntity.Ok(entity));
        }
    }
    public class IperRepositoryStorage : IRepository<IperUser, string>
    {
        public Task<BatchResults<IperUser, string>> BatchAsync(BatchOperations<IperUser, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IState<IperUser>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new ArgumentException("dasdsada");
        }

        public Task<IState<IperUser>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Saalsbury");
        }

        public Task<IperUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new IperUser { Id = "2", Name = "d" })!;
        }

        public async Task<IState<IperUser>> InsertAsync(string key, IperUser value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return IState.Default<IperUser>(true);
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, IFilterExpression query, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEntity<IperUser, string>> QueryAsync(IFilterExpression query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IState<IperUser>> UpdateAsync(string key, IperUser value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken);
            return IState.Default<IperUser>(false);
        }
    }
}
