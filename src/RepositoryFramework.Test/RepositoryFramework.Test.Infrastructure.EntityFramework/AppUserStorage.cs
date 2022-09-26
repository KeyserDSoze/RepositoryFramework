using Microsoft.EntityFrameworkCore;
using RepositoryFramework.Test.Domain;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace RepositoryFramework.Test.Infrastructure.EntityFramework
{
    public class AppUserStorage : IRepository<AppUser, AppUserKey>
    {
        private readonly SampleContext _context;

        public AppUserStorage(SampleContext context)
        {
            _context = context;
        }

        public async Task<BatchResults<AppUser, AppUserKey>> BatchAsync(BatchOperations<AppUser, AppUserKey> operations, CancellationToken cancellationToken = default)
        {
            BatchResults<AppUser, AppUserKey> results = new();
            foreach (var operation in operations.Values)
            {
                switch (operation.Command)
                {
                    case CommandType.Delete:
                        results.AddDelete(operation.Key, await DeleteAsync(operation.Key, cancellationToken).NoContext());
                        break;
                    case CommandType.Insert:
                        results.AddInsert(operation.Key, await InsertAsync(operation.Key, operation.Value!, cancellationToken).NoContext());
                        break;
                    case CommandType.Update:
                        results.AddUpdate(operation.Key, await UpdateAsync(operation.Key, operation.Value!, cancellationToken).NoContext());
                        break;
                }
            }
            return results;
        }

        public async ValueTask<TProperty> OperationAsync<TProperty>(
          OperationType<TProperty> operation,
          IFilterExpression query,
          CancellationToken cancellationToken = default)
        {
            var context = query.Apply(_context.Users);
            object? result = null;
            if (operation.Operation == Operations.Count)
            {
                result = await context.CountAsync(cancellationToken);
            }
            else if (operation.Operation == Operations.Min)
            {
                result = await query.ApplyAsSelect(context).MinAsync(cancellationToken).NoContext();
            }
            else if (operation.Operation == Operations.Max)
            {
                result = await query.ApplyAsSelect(context).MaxAsync(cancellationToken).NoContext();
            }
            else if (operation.Operation == Operations.Sum)
            {
                var select = query.GetFirstSelect<User>();
                result = await context.SumAsync(select!.AsExpression<User, decimal>(), cancellationToken).NoContext();
            }
            else if (operation.Operation == Operations.Average)
            {
                var select = query.GetFirstSelect<User>();
                result = await context.AverageAsync(select!.AsExpression<User, decimal>(), cancellationToken).NoContext();
            }
            return result.Cast<TProperty>() ?? default!;
        }

        public async Task<IState<AppUser>> DeleteAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            if (user != null)
            {
                _context.Users.Remove(user);
                return IState.Default<AppUser>(await _context.SaveChangesAsync(cancellationToken) > 0);
            }
            return IState.Default<AppUser>(false);
        }

        public async Task<IState<AppUser>> ExistAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            return IState.Default<AppUser>(user != null);
        }

        public async Task<AppUser?> GetAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            if (user != null)
                return new AppUser(user.Identificativo, user.Nome, user.IndirizzoElettronico, new(), default);
            return default;
        }

        public async Task<IState<AppUser>> InsertAsync(AppUserKey key, AppUser value, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                IndirizzoElettronico = value.Email,
                Nome = value.Username,
                Cognome = string.Empty,
            };
            _context.Users.Add(user);
            return IState.Default(
                await _context.SaveChangesAsync(cancellationToken) > 0,
                value with { Id = user.Identificativo });
        }

        public async IAsyncEnumerable<IEntity<AppUser, AppUserKey>> QueryAsync(IFilterExpression query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var user in query.ApplyAsAsyncEnumerable(_context.Users))
                yield return IEntity.Default(new AppUserKey(user.Identificativo), new AppUser(user.Identificativo, user.Nome, user.IndirizzoElettronico, new(), default));
        }

        public async Task<IState<AppUser>> UpdateAsync(AppUserKey key, AppUser value, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            if (user != null)
            {
                user.Nome = value.Username;
                user.IndirizzoElettronico = value.Email;
                return IState.Default<AppUser>(await _context.SaveChangesAsync(cancellationToken) > 0);
            }
            return IState.Default<AppUser>(false);
        }
    }
}
