namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface IRepositoryBuilder<T> : IRepositoryBuilder<T, string>
    {
        new IQueryTranslationBuilder<T, TTranslated> Translate<TTranslated>();
    }
}