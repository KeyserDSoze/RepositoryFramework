namespace RepositoryFramework
{
    public interface IKey
    {
        string AsString();
        internal static string GetStringedValues(params object[] inputs)
            => string.Join('-', inputs.Select(x => x.ToString()));
    }
}
