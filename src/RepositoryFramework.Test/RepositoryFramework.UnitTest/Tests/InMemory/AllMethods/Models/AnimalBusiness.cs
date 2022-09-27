using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.AllMethods.Models
{
    internal sealed class AnimalBusiness : IRepositoryBusinessBeforeInsert<Animal, long>, IRepositoryBusinessAfterInsert<Animal, long>
    {
        public static int After;
        public Task<IState<Animal>> AfterInsertAsync(IState<Animal> state, IEntity<Animal, long> entity, CancellationToken cancellationToken = default)
        {
            After++;
            return Task.FromResult(state);
        }

        public static int Before;
        public Task<IStatedEntity<Animal, long>> BeforeInsertAsync(IEntity<Animal, long> entity, CancellationToken cancellationToken = default)
        {
            Before++;
            return Task.FromResult(IStatedEntity.Ok(entity));
        }
    }
}
