namespace RepositoryFramework
{
    /// <summary>
    /// Model for your repository service registry.
    /// </summary>
    public class RepositoryFrameworkService
    {
        public Type? RepositoryType { get; set; }
        public Type? CommandType { get; set; }
        public Type? QueryType { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Type KeyType { get; set; }
        public Type StateType { get; set; }
        public Type ModelType { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}