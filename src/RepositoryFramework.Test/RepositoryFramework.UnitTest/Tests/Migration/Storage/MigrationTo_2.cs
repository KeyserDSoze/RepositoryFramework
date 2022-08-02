﻿using RepositoryFramework.UnitTest.Migration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<IEnumerable<SuperMigrationUser>> QueryAsync(QueryOptions<SuperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult(users.AsEnumerable());
        }
        public ValueTask<long> CountAsync(QueryOptions<SuperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return ValueTask.FromResult((long)users.Count());
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
