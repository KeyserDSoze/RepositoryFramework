using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class Repository<T, TKey> : IRepository<T, TKey>, IRepositoryPattern<T, TKey>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        private readonly IRepositoryPattern<T, TKey> _repository;

        public Repository(IRepositoryPattern<T, TKey> repository)
        {
            _repository = repository;
        }
        public Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(key, cancellationToken);

        public Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default) 
            => _repository.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _repository.GetAsync(key, cancellationToken);

        public Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.InsertAsync(key, value, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
            => _repository.QueryAsync(predicate, top, skip, cancellationToken);

        public Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(key, value, cancellationToken);
    }
}