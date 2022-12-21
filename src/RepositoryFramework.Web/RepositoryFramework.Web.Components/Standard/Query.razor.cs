using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using RepositoryFramework.Web.Components.Services;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public PaginationState Pagination { get; set; }
        [Inject]
        public ICopyService Copy { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        private static readonly string? s_editUri = $"Repository/{typeof(T).Name}/Edit/{{0}}";
        private readonly Dictionary<string, ColumnOptions> _columns = new();
        private readonly SearchDictionary _searchDictionary = new();
        private Dictionary<string, PropertyUiSettings> _propertiesRetrieved;

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
            Pagination.TotalItemCountChanged += (sender, eventArgs) => StateHasChanged();
            foreach (var property in TypeShowcase.FlatProperties)
            {
                _columns.Add(property.NavigationPath, new ColumnOptions
                {
                    Type = property.Self.PropertyType,
                    IsActive = true,
                    Label = property.Title,
                    Value = property.NavigationPath
                });
            }
        }
        protected override async Task OnInitializedAsync()
        {
            _propertiesRetrieved =
                ServiceProvider?.GetService<IRepositoryPropertyUiMapper<T, TKey>>() is IRepositoryPropertyUiMapper<T, TKey> uiMapper ?
                await uiMapper.ValuesAsync(ServiceProvider!).NoContext()
                : new();
            await base.OnInitializedAsync().NoContext();
        }
        private string GetEditUri(TKey key)
            => s_editUri != null ? string.Format(s_editUri, key.ToBase64()) : string.Empty;
        private string _lastQueryKey;
        private GridItemsProviderResult<Entity<T, TKey>> _lastQuery;
        private async Task<GridItemsProviderResult<Entity<T, TKey>>> OnReadDataAsync(
            GridItemsProviderRequest<Entity<T, TKey>> request)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"{Pagination.CurrentPageIndex}_{Pagination.ItemsPerPage}_");
            stringBuilder.Append(string.Join('_', _searchDictionary.GetExpressions()));
            stringBuilder.Append($"_{request.SortByColumn?.Title}_{request.SortByAscending}");
            var queryKey = stringBuilder.ToString();
            if (_lastQueryKey != queryKey)
            {
                LoadService.Show();
                _lastQueryKey = queryKey;
                GridItemsProviderResult<Entity<T, TKey>> items = new();
                var queryBuilder = Query.AsQueryBuilder();

                foreach (var expression in _searchDictionary.GetExpressions<T>())
                    queryBuilder.Where(expression);

                if (request.SortByColumn != null)
                {
                    var orderExpression = $"x => x.{request.SortByColumn.Title}".DeserializeAsDynamic(typeof(T));
                    if (request.SortByAscending)
                        queryBuilder = queryBuilder.OrderBy(x => orderExpression.Compile().DynamicInvoke(x));
                    else if (!request.SortByAscending)
                        queryBuilder = queryBuilder.OrderByDescending(x => orderExpression.Compile().DynamicInvoke(x));
                }

                var page = await queryBuilder.PageAsync(Pagination.CurrentPageIndex + 1, Pagination.ItemsPerPage).NoContext();
                items.TotalItemCount = (int)page.TotalCount;
                items.Items = page.Items.ToList();
                LoadService.Hide();
                _lastQuery = items;
            }
            return _lastQuery;
        }
        public async ValueTask GoToPageAsync(int page)
        {
            if (page < 0)
                page = 0;
            else if (page > Pagination.LastPageIndex)
                page = Pagination.LastPageIndex.Value;
            await Pagination.SetCurrentPageIndexAsync(page).NoContext();
        }
        private async Task ShowMoreValuesAsync(Entity<T, TKey>? entity, BaseProperty property)
        {
            var retrieve = Try.WithDefaultOnCatch(() => property.Value(entity, null));
            if (retrieve.Exception == null && retrieve.Entity is IEnumerable enumerable && enumerable.GetEnumerator().MoveNext())
            {
                _ = await DialogService.OpenAsync<Visualizer>(property.Title,
                    new Dictionary<string, object>
                    {
                        { Constant.Entity, retrieve.Entity },
                    }, new DialogOptions
                    {
                        Width = Constant.DialogWidth
                    });
            }
        }
        private IEnumerable<PageWrapper> GetPages()
        {
            for (int i = 0; i <= Pagination.LastPageIndex; i++)
            {
                yield return new PageWrapper
                {
                    Label = i + 1,
                    Value = i,
                };
            }
        }
        private const string EnumerableLabelCount = "Show {0} items.";
        private string EnumerableCountAsString(Entity<T, TKey>? entity, BaseProperty property)
            => string.Format(EnumerableLabelCount, EnumerableCount(entity, property));
        private int EnumerableCount(Entity<T, TKey>? entity, BaseProperty property)
        {
            var response = Try.WithDefaultOnCatch(() => property.Value(entity, null));
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
        private PropertyUiSettings? GetPropertySettings(BaseProperty property)
        {
            var propertyUiSettings = _propertiesRetrieved != null && _propertiesRetrieved.ContainsKey(property.NavigationPath) ? _propertiesRetrieved[property.NavigationPath] : null;
            return propertyUiSettings;
        }
        public void Search()
        {
            StateHasChanged();
        }
        private void NavigateTo(string uri)
        {
            LoadService.Show();
            NavigationManager.NavigateTo(uri);
        }
    }
}
