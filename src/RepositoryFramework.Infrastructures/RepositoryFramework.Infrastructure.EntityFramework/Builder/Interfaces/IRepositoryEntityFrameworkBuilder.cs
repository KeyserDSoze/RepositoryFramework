using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.EntityFramework
{
    public interface IRepositoryEntityFrameworkBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}
