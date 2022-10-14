using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.EntityFramework
{
    public sealed class EntityFrameworkOptions<T, TKey>
        where T : class
    {
        internal static EntityFrameworkOptions<T, TKey> Instance { get; } = new();
        public Func<DbContext, DbSet<T>> Read { get; set; }
        public Func<DbContext, DbSet<T>> ReadWithInclude { get; set; }
        public Func<DbContext, DbSet<T>> Write { get; set; }
        public Func<T, TKey> KeyReader { get; set; }
        public Func<IServiceProvider, DbContext> GetDbContext { get; set; }
    }
}
