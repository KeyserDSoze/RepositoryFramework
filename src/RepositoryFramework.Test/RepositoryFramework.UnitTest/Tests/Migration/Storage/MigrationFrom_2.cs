using RepositoryFramework.Migration;
using RepositoryFramework.UnitTest.Migration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Migration.Storage
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
        public Task<IState<SuperMigrationUser>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(IState.Default<SuperMigrationUser>(_users.Remove(key)));
            return Task.FromResult(IState.Default<SuperMigrationUser>(true));
        }

        public Task<IState<SuperMigrationUser>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(IState.Default<SuperMigrationUser>(_users.ContainsKey(key)));
        }

        public Task<SuperMigrationUser?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_users.ContainsKey(key))
                return Task.FromResult(_users[key])!;
            return Task.FromResult(default(SuperMigrationUser));
        }

        public Task<IState<SuperMigrationUser>> InsertAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users.Add(key, value);
            return Task.FromResult(IState.Default<SuperMigrationUser>(true));
        }

        public async IAsyncEnumerable<IEntity<SuperMigrationUser, string>> QueryAsync(IFilterExpression filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var users = filter.Apply(_users.Select(x => x.Value));
            await foreach (var user in users.ToAsyncEnumerable())
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return IEntity.Default(user.Id!, user);
            }
        }
        public Task<IState<SuperMigrationUser>> UpdateAsync(string key, SuperMigrationUser value, CancellationToken cancellationToken = default)
        {
            _users[key] = value;
            return Task.FromResult(IState.Default<SuperMigrationUser>(true));
        }

        public Task<BatchResults<SuperMigrationUser, string>> BatchAsync(BatchOperations<SuperMigrationUser, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, IFilterExpression filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
