using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public bool EditableKey { get; set; } = true;
        [Parameter]
        public int PageSize { get; set; } = 10;
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
        private readonly Dictionary<string, bool> _check = new();
        protected override async Task OnParametersSetAsync()
        {
            if (!_alreadySet)
            {
                _alreadySet = true;
                if (Repository != null)
                {
                    _canEdit = true;
                    _entities = await Repository.ToListAsync().NoContext();
                    Command = Repository;
                    Queryx = Repository;
                }
                else if (Queryx != null)
                {
                    _entities = await Queryx.ToListAsync().NoContext();
                }
                else if (Command != null)
                    _canEdit = true;
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
    }
}
