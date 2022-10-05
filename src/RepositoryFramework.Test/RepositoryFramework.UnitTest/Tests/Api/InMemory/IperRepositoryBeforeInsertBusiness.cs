using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.Test.Models
{
    public class IperRepositoryBeforeInsertBusiness : IRepositoryBusinessBeforeInsert<IperUser, string>
    {
        public async Task<State<IperUser, string>> BeforeInsertAsync(Entity<IperUser, string> entity, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}
