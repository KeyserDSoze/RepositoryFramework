using RepositoryFramework.Migration;
using RepositoryFramework.UnitTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Storage
{
    internal class MigrationTo : IRepositoryPattern<MigrationUser>
    {
        private readonly Dictionary<string, MigrationUser> _users = new();
        public Task<State> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(new State(_users.Remove(key)));
            return Task.FromResult(new State(true));
        }

        public Task<State> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new State(_users.ContainsKey(key)));
        }

        public Task<MigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(MigrationUser));
        }

        public Task<State> InsertAsync(string key, MigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(new State(true));
        }

        public Task<IEnumerable<MigrationUser>> QueryAsync(QueryOptions<MigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult(users);
        }
        public Task<long> CountAsync(QueryOptions<MigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult((long)users.Count());
        }
        public Task<State> UpdateAsync(string key, MigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(new State(true));
        }

        public Task<List<BatchResult<string, State>>> BatchAsync(List<BatchOperation<MigrationUser, string>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
