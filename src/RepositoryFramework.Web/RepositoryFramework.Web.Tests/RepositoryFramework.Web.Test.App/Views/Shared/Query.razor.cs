namespace RepositoryFramework.Web.Components
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        private List<Entity<T, TKey>> _entities;
        private readonly IRepository<T, TKey> _repository;
        private Entity<T, TKey> _selectedEntity;
        public Query(IRepository<T, TKey> repository)
        {
            _repository = repository;
            _entities = new();
        }
        protected override async Task OnInitializedAsync()
        {
            _entities = await _repository.ToListAsync();
        }
    }
}
