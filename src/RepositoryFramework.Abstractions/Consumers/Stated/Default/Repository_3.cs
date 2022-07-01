using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class Repository<T, TKey, TState> : IRepository<T, TKey, TState>, IRepositoryPattern<T, TKey, TState>, ICommandPattern<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey, TState> _repository;

        public Repository(IRepositoryPattern<T, TKey, TState> repository)
        {
            _repository = repository;
        }
        public Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(key, cancellationToken);

        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default) 
            => _repository.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.GetAsync(key, cancellationToken);

        public Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.InsertAsync(key, value, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _repository.QueryAsync(options, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(key, value, cancellationToken);
    }
}