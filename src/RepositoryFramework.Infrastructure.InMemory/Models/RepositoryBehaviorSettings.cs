namespace RepositoryFramework.InMemory
{
    /// <summary>
    /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
    /// You may set a list of exceptions with a random percentage of throwing.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public class RepositoryBehaviorSettings<T, TKey, TState>
        where TKey : notnull
    {
        private readonly Dictionary<RepositoryMethod, MethodBehaviorSetting> _settings = new();
        internal Func<bool, Exception, TState>? PopulationOfState { get; set; }
        internal int? NumberOfParameters { get; set; }
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
        }
        public void AddForQueryPattern(MethodBehaviorSetting setting)
        {
            Add(RepositoryMethod.Get, setting);
            Add(RepositoryMethod.Query, setting);
            Add(RepositoryMethod.Exist, setting);
        }
        public void AddForInsert(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Insert, setting);
        public void AddForUpdate(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Update, setting);
        public void AddForDelete(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Delete, setting);
        public void AddForGet(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Get, setting);
        public void AddForQuery(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Query, setting);
        public void AddForExist(MethodBehaviorSetting setting)
            => Add(RepositoryMethod.Exist, setting);
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