namespace RepositoryFramework
{
    public record Key<T1>(T1 Primary) : IKey
        where T1 : notnull
    {
        public string AsString()
            => GetStringedValues(Primary);
        private protected static string GetStringedValues(params object[] inputs)
            => string.Join('-', inputs.Select(x => x.ToString()));
    }
}
