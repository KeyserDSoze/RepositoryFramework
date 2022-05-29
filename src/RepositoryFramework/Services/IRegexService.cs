namespace RepositoryFramework.Services
{
    public interface IRegexService
    {
        dynamic GetRandomValue(Type type, string[] pattern);
    }
}
