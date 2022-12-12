using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Radzen;
using RepositoryFramework.Web.Components.Services;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public int PageSize { get; set; } = 10;
        [Inject]
        public ICopyService Copy { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        private bool _isLoading = true;
        private List<Entity<T, TKey>>? _entities;
        private int _totalItems;
        private static readonly string? s_editUri = $"Repository/{typeof(T).Name}/Edit/{{0}}";
        private static readonly List<int> s_pageSizeOptions = new() { 10, 20, 50, 100, 200 };
        private string GetEditUri(TKey key)
            => s_editUri != null ? string.Format(s_editUri, key.ToBase64()) : string.Empty;
        private async Task OnReadData(LoadDataArgs args)
        {
            _isLoading = true;
            if (Query != null)
            {
                var actualPage = (args.Top.Value + args.Skip.Value) / PageSize;
                var queryBuilder = Query.AsQueryBuilder();
                if (args.Filters.Any())
                {
                    StringBuilder whereBuilder = new();
                    var properties = typeof(T).FetchProperties();
                    foreach (var filter in args.Filters)
                    {
                        var property = properties.First(x => x.Name == filter.Property);
                        foreach (var value in Values())
                        {
                            whereBuilder.Append(whereBuilder.Length <= 0 ? "x => " : " AndAlso ");
                            whereBuilder.Append(GetValidOperation(property.Name, value.Operator, value.Value, property.PropertyType.IsNumeric()));
                        }
                        IEnumerable<(object Value, FilterOperator Operator)> Values()
                        {
                            yield return (filter.FilterValue, filter.FilterOperator);
                            if (filter.SecondFilterValue != null || filter.FilterOperator == FilterOperator.IsNull)
                                yield return (filter.SecondFilterValue, filter.SecondFilterOperator);
                        }
                        string GetValidOperation(string name, FilterOperator op, object? value, bool isNumeric)
                        {
                            var stringedValue = value?.ToString();
                            if (!isNumeric)
                                stringedValue = $"\"{stringedValue}\"";
                            return op switch
                            {
                                FilterOperator.GreaterThan => $"x.{name} > {stringedValue}",
                                FilterOperator.LessThan => $"x.{name} < {stringedValue}",
                                FilterOperator.GreaterThanOrEquals => $"x.{name} >= {stringedValue}",
                                FilterOperator.LessThanOrEquals => $"x.{name} <= {stringedValue}",
                                FilterOperator.NotEquals => $"x.{name} != {stringedValue}",
                                FilterOperator.Contains => $"x.{name}.Contains({stringedValue})",
                                FilterOperator.DoesNotContain => $"!x.{name}.Contains({stringedValue})",
                                FilterOperator.EndsWith => $"!x.{name}.EndsWith({stringedValue})",
                                FilterOperator.StartsWith => $"!x.{name}.StartsWith({stringedValue})",
                                FilterOperator.IsNull => $"!x.{name} == null",
                                FilterOperator.IsNotNull => $"!x.{name} != null",
                                _ => $"x.{name} == {stringedValue}",
                            };
                        }
                    }
                    var whereExpression = whereBuilder.ToString().Deserialize<T, bool>();
                    queryBuilder = queryBuilder.Where(whereExpression);
                }
                if (!string.IsNullOrWhiteSpace(args.OrderBy))
                {
                    var orderExpression = $"x => x.{args.OrderBy.Split(' ').First()}".DeserializeAsDynamic(typeof(T));
                    if (args.OrderBy.EndsWith(" desc"))
                        queryBuilder = queryBuilder.OrderByDescending(x => orderExpression.Compile().DynamicInvoke(x));
                    else
                        queryBuilder = queryBuilder.OrderBy(x => orderExpression.Compile().DynamicInvoke(x));
                }
                var page = await queryBuilder.PageAsync(actualPage, PageSize).NoContext();
                _totalItems = (int)page.TotalCount;
                _entities = page.Items.ToList();
            }
            _isLoading = false;
        }
        private string GetKey(Entity<T, TKey> entity)
            => entity!.Key!.GetType().IsPrimitive() ? entity.Key.ToString() : entity.Key.ToJson();
        private async Task ShowMoreValuesAsync(T? entity, BaseProperty property)
        {
            _ = await DialogService.OpenAsync<Visualizer>(property.NavigationPath,
                new Dictionary<string, object>
                {
                    { "Entity", property.Value(entity) },
                }, new DialogOptions
                {
                    Width = "80%"
                });
        }
        private const string EnumerableLabelCount = "Show {0} items.";
        private string EnumerableCountAsString(T? entity, BaseProperty property)
            => string.Format(EnumerableLabelCount, EnumerableCount(entity, property));
        private int EnumerableCount(T? entity, BaseProperty property)
        {
            var items = property.Value(entity);
            if (items == null)
                return 0;
            else if (items is IList list)
                return list.Count;
            else if (items is IQueryable queryable)
                return queryable.Count();
            else if (items.GetType().IsArray)
                return ((dynamic)items).Length;
            else if (items is IEnumerable enumerable)
                return enumerable.AsQueryable().Count();
            return 0;
        }
    }
}
