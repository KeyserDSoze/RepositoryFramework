using System;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.Test.Domain
{
    public class AppUserBeforeInsertBusiness2 : IRepositoryBusinessBeforeInsert<AppUser, AppUserKey>
    {
        public async Task<State<AppUser, AppUserKey>> BeforeInsertAsync(Entity<AppUser, AppUserKey> entity, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            if (entity.Key!.Id == 120)
                throw new UnauthorizedAccessException("you don't have to stay here.");
            return true;
        }
    }
}
