using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components
{
    public partial class RepositoryManager<T, TKey>
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
        [Inject]
        public IRepository<T, TKey>? Repository { get; set; }
        [Inject]
        public IQuery<T, TKey>? Queryx { get; set; }
        [Inject]
        public ICommand<T, TKey>? Command { get; set; }
        private bool _canEdit;
        private bool _alreadySet;
        private int _totalItems;
        private readonly Dictionary<string, bool> _check = new();
        protected override async Task OnParametersSetAsync()
        {
            if (!_alreadySet)
            {
                _alreadySet = true;
                if (Repository != null)
                {
                    Queryx = Repository;
                    Command = Repository;
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
                _canEdit = Command != null;
            }
            await base.OnParametersSetAsync().NoContext();
        }
        private async ValueTask<bool> DeleteAsync(TKey key)
        {
            if (Command != null)
                return await Command.DeleteAsync(key);
            return false;
        }
        private async ValueTask<bool> SaveAsync(Entity<T, TKey> item)
        {
            if (Command != null)
                if (Queryx == null || !await Queryx.ExistAsync(item.Key!))
                    return await Command.InsertAsync(item.Key!, item.Value!);
                else
                    return await Command.UpdateAsync(item.Key!, item.Value!);
            return false;
        }
        private static Entity<T, TKey> NewEntity()
        {
            var entity = new Entity<T, TKey>(typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>(), typeof(TKey).CreateWithDefaultConstructorPropertiesAndField<TKey>());
            return entity;
        }
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
                        await Queryx!.Where(where).PageAsync(e.Page, e.PageSize).NoContext():
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
