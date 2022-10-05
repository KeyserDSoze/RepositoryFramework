namespace RepositoryFramework
{
    public interface IRepositoryFrameworkOptions
    {
        bool HasToTranslate { get; set; }
        bool IsNotExposableAsApi { get; set; }
    }
}
