﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public int PageSize { get; set; } = 10;
        [Parameter]
        public bool Progressive { get; set; }
        [Parameter]
        public Expression<Func<T, bool>>? Prefilter { get; set; }
        private List<Entity<T, TKey>>? _entities;
        private Entity<T, TKey>? _selectedEntity;
        private int _totalItems;
        private string? _createUri;
        private string? _editUri;
        private string? _deleteUri;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            if (CanEdit)
            {
                _createUri = $"Repository/{typeof(T).Name}/Create";
                _editUri = $"Repository/{typeof(T).Name}/Edit/{{0}}";
                _deleteUri = $"Repository/{typeof(T).Name}/Delete/{{0}}";
            }
            if (Query != null)
            {
                if (Progressive)
                {
                    var page =
                        Prefilter == null ?
                        await Query.PageAsync(1, PageSize).NoContext() :
                        await Query.Where(Prefilter).PageAsync(1, PageSize).NoContext();
                    _totalItems = (int)page.TotalCount;
                    _entities = page.Items.ToList();
                }
                else
                {
                    var items =
                        Prefilter == null ?
                        await Query.ToListAsync().NoContext() :
                        await Query.Where(Prefilter).ToListAsync().NoContext();
                    _totalItems = items.Count;
                    _entities = items;
                }
            }
        }
        private string GetCreateUri()
            => _createUri ?? string.Empty;
        private string GetEditUri(TKey key)
            => _editUri != null ? string.Format(_editUri, IKey.AsString(key)) : string.Empty;
        private string GetDeleteUri(TKey key)
            => _deleteUri != null ? string.Format(_deleteUri, IKey.AsString(key)) : string.Empty;
        private async Task OnReadData(DataGridReadDataEventArgs<Entity<T, TKey>> e)
        {
            if (!e.CancellationToken.IsCancellationRequested)
            {
                StringBuilder query = new();
                var properties = typeof(T).FetchProperties();
                foreach (var column in e.Columns.Where(x => x.SearchValue != null))
                {
                    var searchValue = column.SearchValue.ToString();
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        query.Append(query.Length <= 0 ? "x => " : " AndAlso ");
                        var name = column.Field.Replace("Value.", string.Empty, 1);
                        var property = properties.First(x => x.Name == name);
                        if (property.PropertyType.IsNumeric())
                        {
                            query.Append($"x.{name} == {searchValue}");
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            query.Append($"x.{name}.Contains(\"{searchValue}\")");
                        }
                    }
                }
                var nextQuery = query.ToString();
                if (!string.IsNullOrWhiteSpace(nextQuery))
                {
                    var where = nextQuery.Deserialize<T, bool>();
                    var response = Prefilter == null ?
                        await Query!.Where(where).PageAsync(e.Page, e.PageSize).NoContext() :
                        await Query!.Where(Prefilter).Where(where).PageAsync(e.Page, e.PageSize).NoContext();
                    _entities = response.Items!.ToList();
                    _totalItems = (int)response.TotalCount;
                }
                else
                {
                    var response =
                        Prefilter == null ?
                        await Query!.PageAsync(e.Page, e.PageSize).NoContext() :
                        await Query!.Where(Prefilter).PageAsync(e.Page, e.PageSize).NoContext();
                    _entities = response.Items!.ToList();
                    _totalItems = (int)response.TotalCount;
                }
            }
        }
        private string GetRealNavigationPath(string navigationPath)
            => $"{nameof(Entity<T, TKey>.Value)}.{navigationPath}";
        private protected RenderFragment OpenEnumerableVisualizer(T? entity, BaseProperty property)
        {
            if (entity != null)
            {
                var value = property.Value(entity);
                if (value != null)
                {
                    var genericType = typeof(Visualizer<>).MakeGenericType(new[] { property.Self.PropertyType });
                    var frag = new RenderFragment(b =>
                    {
                        b.OpenComponent(1, genericType);
                        b.AddAttribute(2, nameof(Entity), value);
                        b.CloseComponent();
                    });
                    return frag;
                }
            }
            return null;
        }
    }
}
