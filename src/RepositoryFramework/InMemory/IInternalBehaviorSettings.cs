namespace RepositoryFramework
{
    public interface IInternalBehaviorSettings
    {
        Dictionary<string, string[]> RegexForValueCreation { get; set; }
        Dictionary<string, Func<dynamic>> DelegatedMethodForValueCreation { get; set; }
        Dictionary<string, Type> ImplementationForValueCreation { get; set; }
        Dictionary<string, int> NumberOfElements { get; set; }
    }
}