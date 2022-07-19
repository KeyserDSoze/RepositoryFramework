﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AnimalDatabase
    {
        private static readonly Dictionary<int, Animal> _animals = new();
        public Dictionary<int, Animal> Animals => _animals;
    }
    public class AnimalStorage : IRepository<Animal, int>
    {
        private readonly AnimalDatabase _database;
        public AnimalStorage(AnimalDatabase database)
        {
            _database = database;
        }
        public Task<List<BatchResult<int, State<Animal>>>> BatchAsync(List<BatchOperation<Animal, int>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(QueryOptions<Animal>? options = null, CancellationToken cancellationToken = default)
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