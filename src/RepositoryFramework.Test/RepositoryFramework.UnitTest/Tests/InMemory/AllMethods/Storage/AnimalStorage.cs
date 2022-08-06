﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, QueryOptions<Animal>? options = null, System.Linq.Expressions.Expression<Func<Animal, TProperty>>? aggregateExpression = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<Animal> QueryAsync(QueryOptions<Animal>? options = null,
             [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken);
            foreach (var entity in _database.Animals.Filter(options))
                yield return entity;
        }

        public Task<State<Animal>> UpdateAsync(int key, Animal value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}