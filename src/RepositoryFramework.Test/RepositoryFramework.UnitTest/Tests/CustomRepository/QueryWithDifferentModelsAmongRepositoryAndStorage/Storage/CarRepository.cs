using RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage.Storage
{
    public class CarRepository : IRepository<Car, int>
    {
        private readonly List<Auto> _database = new()
        {
            new Auto { Identificativo = 5, Targa = "03djkd0", NumeroRuote = 2, Guidatore = new() },
            new Auto { Identificativo = 1, Targa = "03djks0", NumeroRuote = 4, Guidatore = new() },
            new Auto { Identificativo = 2, Targa = "03djka0", NumeroRuote = 4, Guidatore = new() },
            new Auto { Identificativo = 3, Targa = "03djkb0", NumeroRuote = 3, Guidatore = new() },
            new Auto { Identificativo = 4, Targa = "03djkc0", NumeroRuote = 2, Guidatore = new() },
        };
        public Task<BatchResults<int, State<Car>>> BatchAsync(BatchOperations<Car, int, State<Car>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(QueryOptions<Car>? options = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult((long)_database.Count);
        }

        public Task<State<Car>> DeleteAsync(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<Car>> ExistAsync(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Car?> GetAsync(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<Car>> InsertAsync(int key, Car value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Car>> QueryAsync(QueryOptions<Car>? options = null, CancellationToken cancellationToken = default)
        {
            var filtered = _database.Filter(
                options?
                .Translate<Auto>()
                .With(x => x.Id, x => x.Identificativo)
                .With(x => x.NumberOfWheels, x => x.NumeroRuote)
                .With(x => x.Plate, x => x.Targa)
                .With(x => x.Driver, x => x.Guidatore)
                .With(x => x.Driver!.Name, x => x.Guidatore!.Nome))
                .ToList();
            return Task.FromResult(filtered?.Select(x => new Car { Id = x.Identificativo, Plate = x.Targa, NumberOfWheels = x.NumeroRuote }) ?? new List<Car>());
        }

        public Task<State<Car>> UpdateAsync(int key, Car value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
