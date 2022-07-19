using System.Linq.Expressions;
using System.Security.Cryptography;

namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T, TKey, TState> : IRepositoryPattern<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly RepositoryBehaviorSettings<T, TKey, TState> _settings;
        public InMemoryStorage(RepositoryBehaviorSettings<T, TKey, TState> settings)
        {
            _settings = settings;
        }
        internal static Dictionary<TKey, T> Values { get; } = new();
        private static int GetRandomNumber(Range range)
        {
            int maxPlusOne = range.End.Value + 1 - range.Start.Value;
            return RandomNumberGenerator.GetInt32(maxPlusOne) + range.Start.Value;
        }
        private static Exception? GetException(List<ExceptionOdds> odds)
        {
            if (odds.Count == 0)
                return default;
            var oddBase = (int)Math.Pow(10, odds.Select(x => x.Percentage.ToString()).OrderByDescending(x => x.Length).First().Split('.').Last().Length);
            List<ExceptionOdds> normalizedOdds = new();
            foreach (var odd in odds)
            {
                normalizedOdds.Add(new ExceptionOdds
                {
                    Exception = odd.Exception,
                    Percentage = odd.Percentage * oddBase
                });
            }
            Range range = new(0, 100 * oddBase);
            var result = GetRandomNumber(range);
            int total = 0;
            foreach (var odd in normalizedOdds)
            {
                var value = (int)odd.Percentage;
                if (result >= total && result < total + value)
                    return odd.Exception;
                total += value;
            }
            return default;
        }
        private async Task<TState> ExecuteAsync(RepositoryMethod method, Func<TState> action, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(method);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken).NoContext();
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken).NoContext();
                    return InMemoryStorage<T, TKey, TState>.SetState(false);
                }
                if (!cancellationToken.IsCancellationRequested)
                    return action.Invoke();
                else
                    return InMemoryStorage<T, TKey, TState>.SetState(false);
            }
            else
                return InMemoryStorage<T, TKey, TState>.SetState(false);
        }
        public Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Delete, () =>
                {
                    if (Values.ContainsKey(key))
                        return InMemoryStorage<T, TKey, TState>.SetState(Values.Remove(key));
                    return InMemoryStorage<T, TKey, TState>.SetState(false);
                }, cancellationToken);

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethod.Get);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken).NoContext();
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken).NoContext();
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    return Values.ContainsKey(key) ? Values[key] : default;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        public static TState SetState(bool isOk, T? value = default)
        {
            TState state = new() { IsOk = isOk, Value = value };
            return state;
        }
        public Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Insert, () =>
            {
                if (!Values.ContainsKey(key))
                {
                    Values.Add(key, value);
                    return InMemoryStorage<T, TKey, TState>.SetState(true, value);
                }
                else
                    return InMemoryStorage<T, TKey, TState>.SetState(false, value);
            }, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Update, () =>
            {
                if (Values.ContainsKey(key))
                {
                    Values[key] = value;
                    return InMemoryStorage<T, TKey, TState>.SetState(false, value);
                }
                else
                    return InMemoryStorage<T, TKey, TState>.SetState(false);
            }, cancellationToken);

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethod.Query);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken).NoContext();
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken).NoContext();
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    IEnumerable<T> values = Values.Select(x => x.Value).Filter(options).AsEnumerable();
                    return values;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        public async Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethod.Count);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken).NoContext();
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken).NoContext();
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    IEnumerable<T> values = Values.Select(x => x.Value).Filter(options);
                    return values.Count();
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Exist, () =>
            {
                return InMemoryStorage<T, TKey, TState>.SetState(Values.ContainsKey(key));
            }, cancellationToken);

        public async Task<List<BatchResult<TKey, TState>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default)
        {
            List<BatchResult<TKey, TState>> results = new();
            foreach (var operation in operations)
            {
                switch (operation.Command)
                {
                    case CommandType.Delete:
                        results.Add(new(operation.Command, operation.Key, await DeleteAsync(operation.Key, cancellationToken).NoContext()));
                        break;
                    case CommandType.Insert:
                        results.Add(new(operation.Command, operation.Key, await InsertAsync(operation.Key, operation.Value!, cancellationToken).NoContext()));
                        break;
                    case CommandType.Update:
                        results.Add(new(operation.Command, operation.Key, await UpdateAsync(operation.Key, operation.Value!, cancellationToken).NoContext()));
                        break;
                }
            }
            return results;
        }
    }
}