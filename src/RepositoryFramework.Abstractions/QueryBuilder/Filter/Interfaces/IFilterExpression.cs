using System.Linq.Expressions;

namespace RepositoryFramework
{
    public interface IFilterExpression
    {
        public static IFilterExpression Empty => FilterExpression.Empty;
        List<FilteringOperation> Operations { get; }
        SerializableFilter Serialize();
        string ToKey();
        IFilterExpression Translate<T>();
        IQueryable<T> Apply<T>(IEnumerable<T> enumerable);
        IQueryable<TValue> Apply<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary);
        IQueryable<T> Apply<T>(IQueryable<T> queryable);
        IAsyncEnumerable<T> ApplyAsAsyncEnumerable<T>(IEnumerable<T> enumerable);
        IAsyncEnumerable<T> ApplyAsAsyncEnumerable<T>(IQueryable<T> queryable);
        IQueryable<dynamic> ApplyAsSelect<T>(IEnumerable<T> enumerable);
        IQueryable<dynamic> ApplyAsSelect<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary);
        IQueryable<dynamic> ApplyAsSelect<T>(IQueryable<T> queryable);
        LambdaExpression? GetFirstSelect<T>();
        LambdaExpression? DefaultSelect { get; }
    }
}
