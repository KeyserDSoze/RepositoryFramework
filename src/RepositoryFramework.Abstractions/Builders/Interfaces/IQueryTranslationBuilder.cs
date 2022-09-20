﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework
{
    public interface IQueryTranslationBuilder<T, TKey, TTranslated>
        where TKey : notnull
    {
        IRepositoryBuilder<T, TKey> Builder { get; }
        IServiceCollection Services { get; }
        IQueryTranslationBuilder<T, TKey, TTranslated> With<TProperty, TTranslatedProperty>(Expression<Func<T, TProperty>> property, Expression<Func<TTranslated, TTranslatedProperty>> translatedProperty);
    }
}