namespace RepositoryFramework
{
    public class BehaviorSettings
    {
        public Dictionary<string, string[]> RegexForValueCreation { get; set; } = new();
        public Dictionary<string, Func<dynamic>> DelegatedMethodForValueCreation { get; set; } = new();
        public Dictionary<string, dynamic> AutoIncrementations { get; set; } = new();
        public Dictionary<string, Type> ImplementationForValueCreation { get; set; } = new();
        public Dictionary<string, int> NumberOfElements { get; set; } = new();
    }
}