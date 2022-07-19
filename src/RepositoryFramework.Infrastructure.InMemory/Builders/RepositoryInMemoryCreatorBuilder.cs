using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public class RepositoryInMemoryCreatorBuilder<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly RepositoryInMemoryBuilder<T, TKey, TState> _builder;
        private readonly CreationSettings _internalBehaviorSettings;
        public RepositoryInMemoryCreatorBuilder(
            RepositoryInMemoryBuilder<T, TKey, TState> builder,
            CreationSettings internalBehaviorSettings)
        {
            _builder = builder;
            _internalBehaviorSettings = internalBehaviorSettings;
        }
        private static string GetNameOfProperty<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
            => string.Join(".", navigationPropertyPath.ToString().Split('.').Skip(1))
                .Replace("First().Value.", "Value.")
                .Replace("First().Key.", "Key.")
                .Replace("First().", string.Empty);
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.RegexForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = regex;
            else
                dictionary.Add(nameOfProperty, regex);
            return this;
        }
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.NumberOfElements;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = numberOfElements;
            else
                dictionary.Add(nameOfProperty, numberOfElements);
            return this;
        }
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.DelegatedMethodForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = () => creator.Invoke()!;
            else
                dictionary.Add(nameOfProperty, () => creator.Invoke()!);
            return this;
        }
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.AutoIncrementations;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = start!;
            else
                dictionary.Add(nameOfProperty, start!);
            return this;
        }
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.ImplementationForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = implementationType;
            else
                dictionary.Add(nameOfProperty, implementationType);
            return this;
        }
        public RepositoryInMemoryCreatorBuilder<T, TKey, TState> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath)
            => WithImplementation(navigationPropertyPath, typeof(TEntity));
        public RepositoryInMemoryBuilder<T, TKey, TState> And()
            => _builder;
    }
}