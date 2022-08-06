﻿using RepositoryFramework.UnitTest.Migration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Migration.Storage
{
    internal class SuperMigrationTo : IRepositoryPattern<SuperMigrationUser, string>
    {
        private readonly Dictionary<string, SuperMigrationUser> _users = new();
        public Task<State<SuperMigrationUser>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(new State<SuperMigrationUser>(_users.Remove(key)));
            return Task.FromResult(new State<SuperMigrationUser>(true));
        }

        public Task<State<SuperMigrationUser>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new State<SuperMigrationUser>(_users.ContainsKey(key)));
        }

        public Task<SuperMigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(SuperMigrationUser));
        }

        public Task<State<SuperMigrationUser>> InsertAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(new State<SuperMigrationUser>(true));
        }

        public IAsyncEnumerable<SuperMigrationUser> QueryAsync(QueryOptions<SuperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).FilterAsAsyncEnumerable(options);
            return users;
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(
          OperationType<TProperty> operation,
          QueryOptions<SuperMigrationUser>? options = null,
          Expression<Func<SuperMigrationUser, TProperty>>? aggregateExpression = null,
          CancellationToken cancellationToken = default)
        {
            if (operation.Type == Operations.Count)
            {
                var users = _users.Select(x => x.Value).Filter(options);
                return ValueTask.FromResult((TProperty)(object)users.Count());
            }
            else
                throw new NotImplementedException();
        }
        public Task<State<SuperMigrationUser>> UpdateAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(new State<SuperMigrationUser>(true));
        }

        public Task<BatchResults<SuperMigrationUser, string>> BatchAsync(BatchOperations<SuperMigrationUser, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}