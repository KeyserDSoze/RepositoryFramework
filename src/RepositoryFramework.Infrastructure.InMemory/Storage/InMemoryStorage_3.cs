using System.Collections.Concurrent;
using System.Reflection;
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
        private static ConcurrentDictionary<string, T> Values { get; } = new();
        internal static void AddValue(TKey key, T value) 
            => Values.TryAdd(key.AsString(), value);
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
        private async Task<TState> ExecuteAsync(RepositoryMethods method, Func<TState> action, CancellationToken cancellationToken = default)
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
            => ExecuteAsync(RepositoryMethods.Delete, () =>
                {
                    string keyAsString = key.AsString();
                    if (Values.ContainsKey(keyAsString))
                        return InMemoryStorage<T, TKey, TState>.SetState(Values.TryRemove(keyAsString, out _));
                    return InMemoryStorage<T, TKey, TState>.SetState(false);
                }, cancellationToken);

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethods.Get);
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
                    string keyAsString = key.AsString();
                    return Values.ContainsKey(keyAsString) ? Values[keyAsString] : default;
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
            => ExecuteAsync(RepositoryMethods.Insert, () =>
            {
                string keyAsString = key.AsString();
                if (!Values.ContainsKey(keyAsString))
                {
                    Values.TryAdd(keyAsString, value);
                    return InMemoryStorage<T, TKey, TState>.SetState(true, value);
                }
                else
                    return InMemoryStorage<T, TKey, TState>.SetState(false, value);
            }, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethods.Update, () =>
            {
                string keyAsString = key.AsString();
                if (Values.ContainsKey(keyAsString))
                {
                    Values[keyAsString] = value;
                    return InMemoryStorage<T, TKey, TState>.SetState(true, value);
                }
                else
                    return InMemoryStorage<T, TKey, TState>.SetState(false);
            }, cancellationToken);

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethods.Query);
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
                    IEnumerable<T> values = Values.Filter(options);
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
            var settings = _settings.Get(RepositoryMethods.Count);
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
                    IEnumerable<T> values = Values.Filter(options);
                    return values.Count();
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethods.Exist, () =>
            {
                string keyAsString = key.AsString();
                return InMemoryStorage<T, TKey, TState>.SetState(Values.ContainsKey(keyAsString));
            }, cancellationToken);

        public async Task<BatchResults<TKey, TState>> BatchAsync(BatchOperations<T, TKey, TState> operations, CancellationToken cancellationToken = default)
        {
            BatchResults<TKey, TState> results = new();
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
    }
}