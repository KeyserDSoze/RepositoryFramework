using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly RepositoryBehaviorSettings<T, TKey> _settings;
        public InMemoryStorage(RepositoryBehaviorSettings<T, TKey> settings)
        {
            _settings = settings;

        }
        private static ConcurrentDictionary<string, IEntity<T, TKey>> Values { get; } = new();
        internal static void AddValue(TKey key, T value)
            => Values.TryAdd(key.AsString(), IEntity.Default(key, value));
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
        private async Task<IState<T>> ExecuteAsync(RepositoryMethods method, Func<State<T>> action, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(method);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken).NoContext();
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken).NoContext();
                    return InMemoryStorage<T, TKey>.SetState(false);
                }
                if (!cancellationToken.IsCancellationRequested)
                    return action.Invoke();
                else
                    return InMemoryStorage<T, TKey>.SetState(false);
            }
            else
                return InMemoryStorage<T, TKey>.SetState(false);
        }
        public Task<IState<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethods.Delete, () =>
            {
                string keyAsString = key.AsString();
                if (Values.ContainsKey(keyAsString))
                    return InMemoryStorage<T, TKey>.SetState(Values.TryRemove(keyAsString, out _));
                return InMemoryStorage<T, TKey>.SetState(false);
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
                    return Values.ContainsKey(keyAsString) ? Values[keyAsString].Value : default;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        public static State<T> SetState(bool isOk, T? value = default)
        {
            State<T> state = new(isOk, value);
            return state;
        }
        public Task<IState<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethods.Insert, () =>
            {
                string keyAsString = key.AsString();
                if (!Values.ContainsKey(keyAsString))
                {
                    Values.TryAdd(keyAsString, IEntity.Default(key, value));
                    return InMemoryStorage<T, TKey>.SetState(true, value);
                }
                else
                    return InMemoryStorage<T, TKey>.SetState(false, value);
            }, cancellationToken);

        public Task<IState<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethods.Update, () =>
            {
                string keyAsString = key.AsString();
                if (Values.ContainsKey(keyAsString))
                {
                    Values[keyAsString] = IEntity.Default(key, value);
                    return InMemoryStorage<T, TKey>.SetState(true, value);
                }
                else
                    return InMemoryStorage<T, TKey>.SetState(false);
            }, cancellationToken);

        public async IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(Query query,
             [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
                foreach (var item in query.Filter(Values.Select(x => x.Value.Value)))
                {
                    if (!cancellationToken.IsCancellationRequested)
                        yield return Values.First(x => x.Value.Value.Equals(item)).Value;
                    else
                        throw new TaskCanceledException();
                }
            }
            else
                throw new TaskCanceledException();
        }
        public async ValueTask<TProperty> OperationAsync<TProperty>(
           OperationType<TProperty> operation,
           Query query,
           CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethods.Operation);
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
                    var filterd = query.Filter(Values);
                    var selected = query.FilterAsSelect(filterd);
                    return (await operation.ExecuteAsync(
                        () => Invoke<TProperty>(selected.Count()),
                        () => Invoke<TProperty>(selected.Sum(x => ((object)x).Cast<decimal>())),
                        () => Invoke<TProperty>(selected.Max()!),
                        () => Invoke<TProperty>(selected.Min()!),
                        () => Invoke<TProperty>(selected.Average(x => ((object)x).Cast<decimal>()))))!;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        private static ValueTask<TProperty> Invoke<TProperty>(object value) 
            => ValueTask.FromResult((TProperty)Convert.ChangeType(value, typeof(TProperty)));
        public Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethods.Exist, () =>
            {
                string keyAsString = key.AsString();
                return InMemoryStorage<T, TKey>.SetState(Values.ContainsKey(keyAsString));
            }, cancellationToken);

        public async Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
        {
            BatchResults<T, TKey> results = new();
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