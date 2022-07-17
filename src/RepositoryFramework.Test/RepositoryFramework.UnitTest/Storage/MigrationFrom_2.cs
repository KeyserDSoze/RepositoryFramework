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
    internal class SuperMigrationFrom : IMigrationSource<SuperMigrationUser, string>
    {
        private readonly Dictionary<string, SuperMigrationUser> _users = new()
        {
            { "1", new SuperMigrationUser { Id = "1", Name = "Ale", Email = "Ale@gmail.com", IsAdmin = true } },
            { "2", new SuperMigrationUser { Id = "2", Name = "Alekud", Email = "Alu@gmail.com", IsAdmin = false } },
            { "3", new SuperMigrationUser { Id = "3", Name = "Alessia", Email = "Alo@gmail.com", IsAdmin = false } },
            { "4", new SuperMigrationUser { Id = "4", Name = "Alisandro", Email = "Ali@gmail.com", IsAdmin = false } },
        };
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

        public Task<SuperMigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(SuperMigrationUser));
        }

        public Task<State> InsertAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(new State(true));
        }

        public Task<IEnumerable<SuperMigrationUser>> QueryAsync(QueryOptions<SuperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult(users);
        }
        public Task<long> CountAsync(QueryOptions<SuperMigrationUser>? options = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value).Filter(options);
            return Task.FromResult((long)users.Count());
        }
        public Task<State> UpdateAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(new State(true));
        }

        public Task<List<BatchResult<string, State>>> BatchAsync(List<BatchOperation<SuperMigrationUser, string>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
