using System.Linq.Expressions;

namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query, Count and Exist methods.
    /// This is the interface that you need to extend if you want to create your query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface IQueryPattern<T> : IQueryPattern<T, string>
    {
    }
}