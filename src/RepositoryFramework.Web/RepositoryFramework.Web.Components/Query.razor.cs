using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace RepositoryFramework.Web.Components
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public bool ShowKey { get; set; }
        [Parameter]
        public bool EditableKey { get; set; } = true;
        private List<Entity<T, TKey>> _entities;
        [Inject]
        public IRepository<T, TKey>? Repository { get; set; }
        [Inject]
        public IQuery<T, TKey>? Queryx { get; set; }
        [Inject]
        public ICommand<T, TKey>? Command { get; set; }
        private Entity<T, TKey> _selectedEntity;
        private bool _canEdit;
        protected override async Task OnInitializedAsync()
        {
            if (Repository != null)
            {
                _canEdit = true;
                _entities = await Repository.ToListAsync();
                Command = Repository;
            }
            else if (Queryx != null)
                _entities = await Queryx.ToListAsync();
            else if (Command != null)
                _canEdit = true;
        }
        private async ValueTask<bool> DeleteAsync(TKey key)
        {
            if (Command != null)
                return await Command.DeleteAsync(key);
            return false;
        }
        private async ValueTask<bool> SaveAsync(Entity<T, TKey> item, MouseEventArgs x)
        {
            if (Command != null)
                if (Repository == null || !await Repository.ExistAsync(item.Key))
                    return await Command.InsertAsync(item.Key, item.Value);
                else
                    return await Command.UpdateAsync(item.Key, item.Value);
            return false;
        }
        private Entity<T, TKey> NewEntity()
        {
            var entity = new Entity<T, TKey>(typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>(), typeof(TKey).CreateWithDefaultConstructorPropertiesAndField<TKey>());
            return entity;
        }
    }
}
