using RepositoryFramework.UnitTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Storage
{
    internal class SuperMigrationTo : IRepositoryPattern<SuperMigrationUser, string>
    {
        private readonly Dictionary<string, SuperMigrationUser> _users = new();
        public Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users.Remove(key));
            return Task.FromResult(true);
        }

        public Task<bool> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_users.ContainsKey(key));
        }

        public Task<SuperMigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(SuperMigrationUser));
        }

        public Task<bool> InsertAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<SuperMigrationUser>> QueryAsync(Expression<Func<SuperMigrationUser, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            var users = _users.Select(x => x.Value);
            if (predicate != null)
                users = users.Where(predicate.Compile());
            if (top != null)
                users = users.Take(top.Value);
            if (skip != null)
                users = users.Skip(skip.Value);
            return Task.FromResult(users);
        }

        public Task<bool> UpdateAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(true);
        }
    }
}
