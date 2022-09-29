using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public static class State
    {
        public static State<T, TKey> Ok<T, TKey>(T value, TKey key)
            where TKey : notnull
            => new(true, new Entity<T, TKey>(value, key));
        public static State<T, TKey> Ok<T, TKey>(Entity<T, TKey>? entity = default)
            where TKey : notnull
            => new(true, entity);
        public static State<T, TKey> NotOk<T, TKey>(T value, TKey key)
            where TKey : notnull
            => new(false, new Entity<T, TKey>(value, key));
        public static State<T, TKey> NotOk<T, TKey>(Entity<T, TKey>? entity = default)
         where TKey : notnull
         => new(false, entity);
        public static State<T, TKey> Default<T, TKey>(bool isOk, T value, TKey? key = default)
            where TKey : notnull
            => new(isOk, new Entity<T, TKey>(value, key));
        public static State<T, TKey> Default<T, TKey>(bool isOk, Entity<T, TKey>? entity = default)
         where TKey : notnull
         => new(isOk, entity);
    }
    public class State<T, TKey>
        where TKey : notnull
    {
        public bool IsOk { get; set; }
        public Entity<T, TKey>? Entity { get; set; }
        public int? Code { get; set; }
        public string? Message { get; set; }
        [JsonIgnore]
        public bool HasEntity => Entity != null;
        public State(bool isOk, T? value = default, TKey? key = default, int? code = default, string? message = default)
            : this(isOk, value != null || key != null ? new Entity<T, TKey>(value!, key) : default, code, message)
        {
        }
        public State(bool isOk, Entity<T, TKey>? entity, int? code = default, string? message = default)
        {
            IsOk = isOk;
            if (entity != null)
                Entity = entity;
            Code = code;
            Message = message;
        }
        public static State<T, TKey> Ok() => new(true);
        public static State<T, TKey> NotOk() => new(false);

        public static implicit operator bool(State<T, TKey> state)
            => state.IsOk;
        public static implicit operator State<T, TKey>(bool state)
            => new(state);
    }
}
