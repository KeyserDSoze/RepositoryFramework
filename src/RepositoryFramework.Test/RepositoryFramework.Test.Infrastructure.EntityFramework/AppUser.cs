using Microsoft.EntityFrameworkCore;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models;

namespace RepositoryFramework.Test.Infrastructure.EntityFramework
{
    public record AppUser(int Id, string Username, string Email, List<string> Groups);
    public record AppUserKey(int Id);
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

        public async Task<long> CountAsync(QueryOptions<AppUser>? options = null, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Filter(options?
                .Translate<User>()
                .With(x => x.Id, x => x.Identificativo)
                .With(x => x.Username, x => x.Nome)
                .With(x => x.Email, x => x.IndirizzoElettronico))
                .ToAsyncEnumerable()
                .CountAsync(cancellationToken);
        }

        public async Task<State<AppUser>> DeleteAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            if (user != null)
            {
                _context.Users.Remove(user);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            return false;
        }

        public async Task<State<AppUser>> ExistAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            return user != null;
        }

        public async Task<AppUser?> GetAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            if (user != null)
                return new AppUser(user.Identificativo, user.Nome, user.IndirizzoElettronico, new());
            return default;
        }

        public async Task<State<AppUser>> InsertAsync(AppUserKey key, AppUser value, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                IndirizzoElettronico = value.Email,
                Nome = value.Username,
                Cognome = string.Empty,
            };
            _context.Users.Add(user);
            return new State<AppUser>
            {
                IsOk = await _context.SaveChangesAsync(cancellationToken) > 0,
                Value = value with { Id = user.Identificativo }
            };
        }

        public async Task<List<AppUser>> QueryAsync(QueryOptions<AppUser>? options = null, CancellationToken cancellationToken = default)
        {
            return (await _context.Users.
                FilterAsAsyncEnumerable(options?
                .Translate<User>()
                .With(x => x.Id, x => x.Identificativo)
                .With(x => x.Username, x => x.Nome)
                .With(x => x.Email, x => x.IndirizzoElettronico))
                .ToListAsync(cancellationToken))
                .Select(x => new AppUser(x.Identificativo, x.Nome, x.IndirizzoElettronico, new())).ToList();
        }

        public async Task<State<AppUser>> UpdateAsync(AppUserKey key, AppUser value, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id, cancellationToken);
            if (user != null)
            {
                user.Nome = value.Username;
                user.IndirizzoElettronico = value.Email;
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            return false;
        }
    }
}
