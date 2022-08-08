using RepositoryFramework.UnitTest.Migration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Migration.Storage
{
    internal class MigrationTo : IRepositoryPattern<MigrationUser>
    {
        private readonly Dictionary<string, MigrationUser> _users = new();
        public Task<State<MigrationUser>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(new State<MigrationUser>(_users.Remove(key)));
            return Task.FromResult(new State<MigrationUser>(true));
        }

        public Task<State<MigrationUser>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new State<MigrationUser>(_users.ContainsKey(key)));
        }

        public Task<MigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(MigrationUser));
        }

        public Task<State<MigrationUser>> InsertAsync(string key, MigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(new State<MigrationUser>(true));
        }

        public IAsyncEnumerable<MigrationUser> QueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            var users = query.Filter(_users.Select(x => x.Value));
            return users.ToAsyncEnumerable();
        }

        public Task<State<MigrationUser>> UpdateAsync(string key, MigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(new State<MigrationUser>(true));
        }

        public Task<BatchResults<MigrationUser, string>> BatchAsync(BatchOperations<MigrationUser, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
