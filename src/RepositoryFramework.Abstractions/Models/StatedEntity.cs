namespace RepositoryFramework
{
    public interface IStatedEntity
    {
        public static IStatedEntity<T, TKey> Ok<T, TKey>(TKey key, T entity)
            where TKey : notnull
            => new StatedEntity<T, TKey>(IState.Ok<T>(), IEntity.Default(key, entity));
        public static IStatedEntity<T, TKey> Ok<T, TKey>(IEntity<T, TKey> entity)
            where TKey : notnull
            => new StatedEntity<T, TKey>(IState.Ok<T>(), entity);
    }
    public interface IStatedEntity<out T, out TKey> : IStatedEntity
        where TKey : notnull
    {
        IState<T> State { get; }
        IEntity<T, TKey> Entity { get; }
    }
    internal sealed class StatedEntity<T, TKey> : IStatedEntity<T, TKey>
        where TKey : notnull
    {
        public IState<T> State { get; }
        public IEntity<T, TKey> Entity { get; }
        public StatedEntity(IState<T> state, IEntity<T, TKey> entity)
        {
            State = state;
            Entity = entity;
        }
    }
}
