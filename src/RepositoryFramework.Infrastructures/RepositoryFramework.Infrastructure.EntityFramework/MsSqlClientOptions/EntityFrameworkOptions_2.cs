using Microsoft.EntityFrameworkCore;

namespace RepositoryFramework.Infrastructure.EntityFramework
{
    public sealed class EntityFrameworkOptions<T, TKey, TContext>
        where T : class
        where TKey : notnull
        where TContext : DbContext
    {
        internal static EntityFrameworkOptions<T, TKey, TContext> Instance { get; } = new();
        public Func<TContext, DbSet<T>> DbSet { get; set; }
        public Func<DbSet<T>, IQueryable<T>> IncludingDbSet { get; set; }
        public Func<T, TKey> KeyReader { get; set; }
    }
}
