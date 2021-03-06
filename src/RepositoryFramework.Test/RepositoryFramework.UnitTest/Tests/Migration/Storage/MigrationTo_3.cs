using RepositoryFramework.UnitTest.Migration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Migration.Storage
{
    internal class IperMigrationTo : IRepositoryPattern<IperMigrationUser, string, State<IperMigrationUser>>
    {
        private readonly Dictionary<string, IperMigrationUser> _users = new();
        public Task<State<IperMigrationUser>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(new State<IperMigrationUser>(_users.Remove(key)));
            return Task.FromResult(new State<IperMigrationUser>(true));
        }

        public Task<State<IperMigrationUser>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new State<IperMigrationUser>(_users.ContainsKey(key)));
        }

        public Task<IperMigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(IperMigrationUser));
        }

        public Task<State<IperMigrationUser>> InsertAsync(string key, IperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(new State<IperMigrationUser>(true));
        }

        public Task<IEnumerable<IperMigrationUser>> QueryAsync(QueryOptions<IperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult(users.AsEnumerable());
        }
        public Task<long> CountAsync(QueryOptions<IperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult((long)users.Count());
        }
        public Task<State<IperMigrationUser>> UpdateAsync(string key, IperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(new State<IperMigrationUser>(true));
        }

        public Task<BatchResults<string, State<IperMigrationUser>>> BatchAsync(BatchOperations<IperMigrationUser, string, State<IperMigrationUser>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
