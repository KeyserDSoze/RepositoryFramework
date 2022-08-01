using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class User
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; }
        public User(string email)
        {
            Email = email;
        }
    }
    public class UserWriter : ICommandPattern<User, string>
    {
        public Task<BatchResults<User, string>> BatchAsync(BatchOperations<User, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<User>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
    }
    public class UserReader : IQueryPattern<User, string>
    {
        public Task<long> CountAsync(QueryOptions<User>? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<State<User>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<List<User>> QueryAsync(QueryOptions<User>? options = null, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }
    public class UserRepository : IRepositoryPattern<User, string>
    {
        public Task<State<User>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "Test purpose.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test purpose.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Test purpose.")]
        public Task<IEnumerable<User>> QueryAsync(QueryOptions<User>? options = null, CancellationToken cancellationToken = default)
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var x = options?.Predicate!.ToString();
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }

        public Task<State<User>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(QueryOptions<User>? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BatchResults<User, string>> BatchAsync(BatchOperations<User, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        Task<List<User>> IQueryPattern<User, string>.QueryAsync(QueryOptions<User>? options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal class Solomon
    {
        public int S { get; set; }
        public Range Z { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Folder { get; set; }
        public int Olaf { get; set; }
        public Casualty? Casualty { get; set; }
        public List<int>? Hellos { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
        public int[]? Consolated { get; set; }
    }
    internal class Casualty
    {
        public string? Value { get; set; }
        public string? Folder { get; set; }
    }
}
