﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryCreatorBuilder<T, TKey> : IRepositoryInMemoryCreatorBuilder<T, TKey>
        where TKey : notnull
    {
        private protected readonly RepositoryInMemoryBuilder<T, TKey> _builder;
        private readonly CreationSettings _internalBehaviorSettings;
        public IServiceCollection Services => _builder.Services;
        public RepositoryInMemoryCreatorBuilder(
            RepositoryInMemoryBuilder<T, TKey> builder,
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
        public IRepositoryInMemoryCreatorBuilder<T, TKey> WithPattern<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, params string[] regex)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.RegexForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = regex;
            else
                dictionary.Add(nameOfProperty, regex);
            return this;
        }
        public IRepositoryInMemoryCreatorBuilder<T, TKey> WithSpecificNumberOfElements<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, int numberOfElements)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.NumberOfElements;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = numberOfElements;
            else
                dictionary.Add(nameOfProperty, numberOfElements);
            return this;
        }
        public IRepositoryInMemoryCreatorBuilder<T, TKey> WithValue<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Func<TProperty> creator)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.DelegatedMethodForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = () => creator.Invoke()!;
            else
                dictionary.Add(nameOfProperty, () => creator.Invoke()!);
            return this;
        }
        public IRepositoryInMemoryCreatorBuilder<T, TKey> WithAutoIncrement<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, TProperty start)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.AutoIncrementations;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = start!;
            else
                dictionary.Add(nameOfProperty, start!);
            return this;
        }
        public IRepositoryInMemoryCreatorBuilder<T, TKey> WithImplementation<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath, Type implementationType)
        {
            string nameOfProperty = GetNameOfProperty(navigationPropertyPath);
            var dictionary = _internalBehaviorSettings.ImplementationForValueCreation;
            if (dictionary.ContainsKey(nameOfProperty))
                dictionary[nameOfProperty] = implementationType;
            else
                dictionary.Add(nameOfProperty, implementationType);
            return this;
        }
        public IRepositoryInMemoryCreatorBuilder<T, TKey> WithImplementation<TProperty, TEntity>(Expression<Func<T, TProperty>> navigationPropertyPath)
            => WithImplementation(navigationPropertyPath, typeof(TEntity));
        public IRepositoryInMemoryBuilder<T, TKey> And()
            => _builder;
    }
}