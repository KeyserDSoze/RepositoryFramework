using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Web.Components
{
    public abstract class RepositoryBase<T, TKey> : ComponentBase
        where TKey : notnull
    {
        [Inject]
        public IServiceProvider? ServiceProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public PropertyHandler PropertyHandler { get; set; } = null!;
        protected IRepository<T, TKey>? Repository { get; private set; }
        protected IQuery<T, TKey>? Query { get; private set; }
        protected ICommand<T, TKey>? Command { get; private set; }
        private protected TypeShowcase TypeShowcase { get; set; } = null!;
        private protected bool CanEdit { get; set; }
        protected override void OnInitialized()
        {
            TypeShowcase = PropertyHandler.GetEntity(typeof(Entity<T, TKey>));
            base.OnInitialized();
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().NoContext();
        }
        protected override Task OnParametersSetAsync()
        {
            Repository = ServiceProvider?.GetService<IRepository<T, TKey>>();
            if (Repository != null)
            {
                Query = Repository;
                Command = Repository;
            }
            else
            {
                Query = ServiceProvider?.GetService<IQuery<T, TKey>>();
                Command = ServiceProvider?.GetService<ICommand<T, TKey>>();
            }
            CanEdit = Command != null;
            return base.OnParametersSetAsync();
        }
    }
}
