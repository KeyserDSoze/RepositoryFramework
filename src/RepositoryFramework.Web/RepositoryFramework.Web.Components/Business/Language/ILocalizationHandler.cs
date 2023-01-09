namespace RepositoryFramework.Web.Components.Business.Language
{
    public interface ILocalizationHandler
    {
        string Get(string value);
        string Get<T>(string value);
    }
}
