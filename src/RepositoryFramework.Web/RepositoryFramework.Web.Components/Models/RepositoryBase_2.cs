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
        public PropertyBringer PropertyBringer { get; set; }
        private protected TypeShowcase TypeShowcase { get; set; }
        private protected bool CanEdit { get; set; }
        private bool _alreadySet;
        protected override Task OnInitializedAsync()
        {
            if (!_alreadySet)
            {
                _alreadySet = true;
                TypeShowcase = PropertyBringer.GetEntity(typeof(T));
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
