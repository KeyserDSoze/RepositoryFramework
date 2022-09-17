using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.AllMethods.Models
{
    internal class AnimalBusiness : IRepositoryBusinessBeforeInsert<Animal, long>, IRepositoryBusinessAfterInsert<Animal, long>
    {
        public static int After;
        public Task<IState<Animal>> AfterInsertAsync(IState<Animal> state, long key, Animal value, CancellationToken cancellationToken = default)
        {
            After++;
            return Task.FromResult(state);
        }
        public static int Before;
        public Task<IEntity<Animal, long>> BeforeInsertAsync(IEntity<Animal, long> entity, CancellationToken cancellationToken = default)
        {
            Before++;
            return Task.FromResult(entity);
        }
    }
}
