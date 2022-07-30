namespace RepositoryFramework.InMemory
{
    /// <summary>
    /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
    /// You may set a list of exceptions with a random percentage of throwing.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "It's not used but it's needed for the return methods that use this class.")]
    public class RepositoryBehaviorSettings<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly Dictionary<RepositoryMethod, MethodBehaviorSetting> _settings = new();
        internal RepositoryBehaviorSettings() { }
        private void Add(RepositoryMethod method, MethodBehaviorSetting methodSettings)
        {
            if (!_settings.ContainsKey(method))
                _settings.Add(method, methodSettings);
            else
                _settings[method] = methodSettings;
        }
        public void AddForRepositoryPattern(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.All, setting);
        public void AddForCommandPattern(MethodBehaviorSetting setting)
        {
            Add(RepositoryMethod.Insert, setting);
            Add(RepositoryMethod.Update, setting);
            Add(RepositoryMethod.Delete, setting);
            Add(RepositoryMethod.Batch, setting);
        }
        public void AddForQueryPattern(MethodBehaviorSetting setting)
        {
            Add(RepositoryMethod.Get, setting);
            Add(RepositoryMethod.Query, setting);
            Add(RepositoryMethod.Exist, setting);
            Add(RepositoryMethod.Count, setting);
        }
        public void AddForInsert(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Insert, setting);
        public void AddForUpdate(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Update, setting);
        public void AddForDelete(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Delete, setting);
        public void AddForBatch(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Batch, setting);
        public void AddForGet(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Get, setting);
        public void AddForQuery(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Query, setting);
        public void AddForExist(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Exist, setting);
        public void AddForCount(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Count, setting);
        public MethodBehaviorSetting Get(RepositoryMethod method)
        {
            if (_settings.ContainsKey(method))
                return _settings[method];
            else if (_settings.ContainsKey(RepositoryMethod.All))
                return _settings[RepositoryMethod.All];
            else
                return MethodBehaviorSetting.Default;
        }
    }
}