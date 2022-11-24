using System.Linq.Expressions;
using System.Text;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public bool EditableKey { get; set; } = true;
        [Parameter]
        public int PageSize { get; set; } = 10;
        [Parameter]
        public bool Progressive { get; set; }
        [Parameter]
        public Expression<Func<T, bool>>? Prefilter { get; set; }
        private List<Entity<T, TKey>>? _entities;
        private Entity<T, TKey>? _selectedEntity;
        private int _totalItems;
        private readonly Dictionary<string, bool> _check = new();
        private string _createUri;
        private string _editUri;
        private string _deleteUri;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            if (CanEdit)
            {
                _createUri = $"Repository/{typeof(T).Name}/Create";
                _editUri = $"Repository/{typeof(T).Name}/Edit/{{0}}";
                _deleteUri = $"Repository/{typeof(T).Name}/Delete/{{0}}";
            }
            if (Queryx != null)
            {
                if (Progressive)
                {
                    var page =
                        Prefilter == null ?
                        await Queryx.PageAsync(1, PageSize).NoContext() :
                        await Queryx.Where(Prefilter).PageAsync(1, PageSize).NoContext();
                    _totalItems = (int)page.TotalCount;
                    _entities = page.Items.ToList();
                }
                else
                {
                    var items =
                        Prefilter == null ?
                        await Queryx.ToListAsync().NoContext() :
                        await Queryx.Where(Prefilter).ToListAsync().NoContext();
                    _totalItems = items.Count;
                    _entities = items;
                }
            }
        }
        private void CheckComplexProperties(List<PropertyInfoKeeper> infos, PropertyTree tree)
        {
            infos.AddRange(tree.Primitives);
            foreach (var property in tree.Complexes)
                CheckComplexProperties(infos, property);
        }
        private string GetCreateUri()
            => _createUri;
        private string GetEditUri(TKey key)
            => string.Format(_editUri, IKey.AsString(key));
        private string GetDeleteUri(TKey key)
            => string.Format(_deleteUri, IKey.AsString(key));
        private async Task OnReadData(DataGridReadDataEventArgs<Entity<T, TKey>> e)
        {
            if (!e.CancellationToken.IsCancellationRequested)
            {
                StringBuilder query = new();
                foreach (var column in e.Columns.Where(x => x.SearchValue != null))
                {
                    var name = column.Field.Replace("Value.", string.Empty, 1);
                    if (query.Length <= 0)
                    {
                        query.Append("x => ");
                        query.Append($"x.{name}.Contains(\"{column.SearchValue}\")");
                    }
                    else
                        query.Append($" AndAlso x.{name}.Contains(\"{column.SearchValue}\")");
                }
                var nextQuery = query.ToString();
                if (!string.IsNullOrWhiteSpace(nextQuery))
                {
                    var where = nextQuery.Deserialize<T, bool>();
                    var response = Prefilter == null ?
                        await Queryx!.Where(where).PageAsync(e.Page, e.PageSize).NoContext() :
                        await Queryx!.Where(Prefilter).Where(where).PageAsync(e.Page, e.PageSize).NoContext();
                    _entities = response.Items!.ToList();
                    _totalItems = (int)response.TotalCount;
                }
                else
                {
                    var response =
                        Prefilter == null ?
                        await Queryx!.PageAsync(e.Page, e.PageSize).NoContext() :
                        await Queryx!.Where(Prefilter).PageAsync(e.Page, e.PageSize).NoContext();
                    _entities = response.Items!.ToList();
                    _totalItems = (int)response.TotalCount;
                }
            }
        }
    }
}
