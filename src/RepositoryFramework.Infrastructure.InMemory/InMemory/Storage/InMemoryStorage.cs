using System.Linq.Expressions;
using System.Security.Cryptography;

namespace RepositoryFramework
{
    internal class InMemoryStorage<T, TKey> : IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        private readonly RepositoryBehaviorSettings<T, TKey> _settings;

        public InMemoryStorage(RepositoryBehaviorSettings<T, TKey> settings)
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
        public async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForDelete), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(_settings.ExceptionOddsForDelete);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitBeforeExceptionForDelete), cancellationToken);
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (Values.ContainsKey(key))
                        return Values.Remove(key);
                    return false;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForGet), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(_settings.ExceptionOddsForGet);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitBeforeExceptionForGet), cancellationToken);
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

        public async Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForInsert), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(_settings.ExceptionOddsForInsert);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitBeforeExceptionForInsert), cancellationToken);
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (!Values.ContainsKey(key))
                    {
                        Values.Add(key, value);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
        public async Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForUpdate), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(_settings.ExceptionOddsForUpdate);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitBeforeExceptionForUpdate), cancellationToken);
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (Values.ContainsKey(key))
                    {
                        Values[key] = value;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForQuery), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(_settings.ExceptionOddsForQuery);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitBeforeExceptionForQuery), cancellationToken);
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    IEnumerable<T> values = Values.Select(x => x.Value);
                    if (predicate != null)
                        values = values.Where(predicate.Compile());
                    if (top != null && top > 0)
                        values = values.Take(top.Value);
                    if (skip != null && skip > 0)
                        values = values.Skip(skip.Value);
                    return values;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }
    }
}