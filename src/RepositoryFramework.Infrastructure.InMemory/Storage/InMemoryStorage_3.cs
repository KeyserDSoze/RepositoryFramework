﻿using System.Linq.Expressions;
using System.Security.Cryptography;

namespace RepositoryFramework.InMemory
{
    internal class InMemoryStorage<T, TKey, TState> : IRepositoryPattern<T, TKey, TState>
        where TKey : notnull
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
        private async Task<TState> ExecuteAsync(RepositoryMethod method, Func<bool> action, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(method);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken);
                    return _settings.PopulationOfState!.Invoke(false, exception)!;
                }
                if (!cancellationToken.IsCancellationRequested)
                    return _settings.PopulationOfState!.Invoke(action.Invoke(), null!)!;
                else
                    return _settings.PopulationOfState!.Invoke(false, new TaskCanceledException())!;
            }
            else
                return _settings.PopulationOfState!.Invoke(false, new TaskCanceledException())!;
        }
        public Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Delete, () =>
                {
                    if (Values.ContainsKey(key))
                        return Values.Remove(key);
                    return false;
                }, cancellationToken);

        public async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethod.Get);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken);
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

        public Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Insert, () =>
            {
                if (!Values.ContainsKey(key))
                {
                    Values.Add(key, value);
                    return true;
                }
                else
                    return false;
            }, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Update, () =>
            {
                if (Values.ContainsKey(key))
                {
                    Values[key] = value;
                    return true;
                }
                else
                    return false;
            }, cancellationToken);

        public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            var settings = _settings.Get(RepositoryMethod.Query);
            await Task.Delay(GetRandomNumber(settings.MillisecondsOfWait), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                var exception = GetException(settings.ExceptionOdds);
                if (exception != null)
                {
                    await Task.Delay(GetRandomNumber(settings.MillisecondsOfWaitWhenException), cancellationToken);
                    throw exception;
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    IEnumerable<T> values = Values.Select(x => x.Value).Filter(options);
                    return values;
                }
                else
                    throw new TaskCanceledException();
            }
            else
                throw new TaskCanceledException();
        }

        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => ExecuteAsync(RepositoryMethod.Update, () =>
            {
                return Values.ContainsKey(key);
            }, cancellationToken);
    }
}