using RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage.Storage
{
    public class CarRepository : IRepository<Car, int>
    {
        private readonly List<Auto> _database = new()
        {
            new Auto { Identificativo = 5, Identificativo2 = 5, Targa = "03djkd0", NumeroRuote = 2, Guidatore = new() },
            new Auto { Identificativo = 1, Identificativo2 = 1, Targa = "03djks0", NumeroRuote = 4, Guidatore = new() },
            new Auto { Identificativo = 2, Identificativo2 = 2, Targa = "03djka0", NumeroRuote = 4, Guidatore = new() },
            new Auto { Identificativo = 3, Identificativo2 = 3, Targa = "03djkb0", NumeroRuote = 3, Guidatore = new() },
            new Auto { Identificativo = 4, Identificativo2 = 4, Targa = "03djkc0", NumeroRuote = 2, Guidatore = new() },
        };
        public Task<BatchResults<Car, int>> BatchAsync(BatchOperations<Car, int> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(
          OperationType<TProperty> operation,
          QueryOptions<Car>? options = null,
          Expression<Func<Car, TProperty>>? aggregateExpression = null,
          CancellationToken cancellationToken = default)
        {
            if (operation.Type == Operations.Count)
                return ValueTask.FromResult((TProperty)Convert.ChangeType(_database.Count, typeof(TProperty)));
            else
                throw new NotImplementedException();
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

        public async IAsyncEnumerable<Car> QueryAsync(QueryOptions<Car>? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken);
            var filtered = _database.FilterWithTranslation(
                options)
                .ToList();
            foreach (var item in filtered?.Select(x => new Car { Id = x.Identificativo, Plate = x.Targa, NumberOfWheels = x.NumeroRuote }) ?? new List<Car>())
                yield return item;
        }

        public Task<State<Car>> UpdateAsync(int key, Car value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
