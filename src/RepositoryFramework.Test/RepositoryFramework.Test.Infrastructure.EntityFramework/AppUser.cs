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

        public async Task<List<BatchResult<AppUserKey, State<AppUser>>>> BatchAsync(List<BatchOperation<AppUser, AppUserKey>> operations, CancellationToken cancellationToken = default)
        {
            var results = new List<BatchResult<AppUserKey, State<AppUser>>>();
            foreach (var operation in operations)
            {
                if (operation.Command == CommandType.Insert)
                    results.Add(new BatchResult<AppUserKey, State<AppUser>>(operation.Command, operation.Key, await InsertAsync(operation.Key, operation.Value!, cancellationToken)));
                else if (operation.Command == CommandType.Update)
                    results.Add(new BatchResult<AppUserKey, State<AppUser>>(operation.Command, operation.Key, await UpdateAsync(operation.Key, operation.Value!, cancellationToken)));
                else if (operation.Command == CommandType.Delete)
                    results.Add(new BatchResult<AppUserKey, State<AppUser>>(operation.Command, operation.Key, await DeleteAsync(operation.Key, cancellationToken)));
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
                .CountAsync();
        }

        public async Task<State<AppUser>> DeleteAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<State<AppUser>> ExistAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id);
            return user != null;
        }

        public async Task<AppUser?> GetAsync(AppUserKey key, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id);
            return new AppUser(user.Identificativo, user.Nome, user.IndirizzoElettronico, new());
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
                IsOk = await _context.SaveChangesAsync() > 0,
                Value = value with { Id = user.Identificativo }
            };
        }

        public async Task<IEnumerable<AppUser>> QueryAsync(QueryOptions<AppUser>? options = null, CancellationToken cancellationToken = default)
        {
            return (await _context.Users.
                FilterAsAsyncEnumerable(options?
                .Translate<User>()
                .With(x => x.Id, x => x.Identificativo)
                .With(x => x.Username, x => x.Nome)
                .With(x => x.Email, x => x.IndirizzoElettronico))
                .ToListAsync())
                .Select(x => new AppUser(x.Identificativo, x.Nome, x.IndirizzoElettronico, new()));
        }

        public async Task<State<AppUser>> UpdateAsync(AppUserKey key, AppUser value, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Identificativo == key.Id);
            if (user != null)
            {
                user.Nome = value.Username;
                user.IndirizzoElettronico = value.Email;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
