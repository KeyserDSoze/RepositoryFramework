using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RepositoryFramework.UnitTest.AllMethods.Models;

namespace RepositoryFramework.UnitTest.AllMethods.Storage
{
    public class AnimalStorage : IRepository<Animal, int>
    {
        private readonly AnimalDatabase _database;
        public AnimalStorage(AnimalDatabase database)
        {
            _database = database;
        }
        public Task<BatchResults<Animal, int>> BatchAsync(BatchOperations<Animal, int> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<long> CountAsync(QueryOptions<Animal>? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<Animal>> DeleteAsync(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<Animal>> ExistAsync(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Animal?> GetAsync(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<Animal>> InsertAsync(int key, Animal value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Animal>> QueryAsync(QueryOptions<Animal>? options = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_database.Animals.Select(x => x.Value).Filter(options).AsEnumerable());
        }

        public Task<State<Animal>> UpdateAsync(int key, Animal value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
