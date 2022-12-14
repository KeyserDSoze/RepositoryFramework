using System.Collections;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
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
        private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
        private static readonly string? s_editUri = $"Repository/{typeof(T).Name}/Edit/{{0}}";
        private Dictionary<string, ColumnOptions> _columns = new();

        private sealed class ColumnOptions
        {
            public bool IsActive { get; set; } = true;
            public Type Type { get; set; }
            public string Value { get; set; }
        }
        private void UpdateColumnsVisibility(object keys)
        {
            if (keys is IEnumerable<string> values)
            {
                foreach (var item in _columns)
                    item.Value.IsActive = false;
                foreach (var key in values)
                    _columns[key].IsActive = true;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            pagination.TotalItemCountChanged += (sender, eventArgs) => StateHasChanged();
            _columns.Add("Key", new()
            {
                Type = typeof(TKey),
            });
            foreach (var property in TypeShowcase.FlatProperties)
            {
                _columns.Add(property.NavigationPath, new ColumnOptions
                {
                    Type = property.Self.PropertyType
                });
            }
        }
        private string GetEditUri(TKey key)
            => s_editUri != null ? string.Format(s_editUri, key.ToBase64()) : string.Empty;
        private async Task<GridItemsProviderResult<Entity<T, TKey>>> OnReadDataAsync(GridItemsProviderRequest<Entity<T, TKey>> request)
        {
            LoadService.Show();
            GridItemsProviderResult<Entity<T, TKey>> items = new();
            var page = await Query.PageAsync(1 + request.StartIndex / pagination.ItemsPerPage, pagination.ItemsPerPage).NoContext();
            items.TotalItemCount = (int)page.TotalCount;
            items.Items = page.Items.ToList();
            LoadService.Hide();
            return items;
        }
        private string GetKey(Entity<T, TKey> entity)
            => entity!.Key!.GetType().IsPrimitive() ? entity.Key.ToString() : entity.Key.ToJson();
        private async Task ShowMoreValuesAsync(T? entity, BaseProperty property)
        {
            _ = await DialogService.OpenAsync<Visualizer>(property.NavigationPath,
                new Dictionary<string, object>
                {
                    { "Entity", Try.WithDefaultOnCatch(() => property.Value(entity)).Entity },
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
            var response = Try.WithDefaultOnCatch(() => property.Value(entity));
            if (response.Exception != null)
                return -1;
            var items = response.Entity;
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
