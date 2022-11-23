using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components
{
    public abstract class RepositoryBase<T, TKey> : ComponentBase
        where TKey : notnull
    {
        [Inject]
        public IRepository<T, TKey>? Repository { get; set; }
        [Inject]
        public IQuery<T, TKey>? Queryx { get; set; }
        [Inject]
        public ICommand<T, TKey>? Command { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public EntitiesTypeManager EntityManager { get; set; }
        private protected EntityType PropertyTree { get; set; }
        private protected bool CanEdit { get; set; }
        private bool _alreadySet;
        protected override Task OnInitializedAsync()
        {
            if (!_alreadySet)
            {
                _alreadySet = true;
                PropertyTree = EntityManager.GetEntity(typeof(T));
                if (Repository != null)
                {
                    Queryx = Repository;
                    Command = Repository;
                }
                CanEdit = Command != null;
            }
            return base.OnInitializedAsync();
        }
    }
}
