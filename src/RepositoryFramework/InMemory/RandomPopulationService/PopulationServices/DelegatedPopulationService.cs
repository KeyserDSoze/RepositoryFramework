namespace RepositoryFramework.Population
{
    internal class DelegatedPopulationService<T, TKey> : IDelegatedPopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
            => args.Invoke();
    }
}