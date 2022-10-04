namespace RepositoryFramework.Api.Client
{
    internal sealed class RepositorySingleClientSettings
    {
        public string StartingPath { get; }
        public string? Version { get; }
        public bool IsJsonableKey { get; }
        public string GetPath { get; }
        public string ExistPath { get; }
        public string QueryPath { get; }
        public string OperationPath { get; }
        public string InsertPath { get; }
        public string UpdatePath { get; }
        public string DeletePath { get; }
        public string BatchPath { get; }
        public RepositorySingleClientSettings(string startingPath, string? version, Type modelType, Type keyType)
        {
            StartingPath = startingPath;
            Version = version;
            if (keyType == typeof(string) || keyType == typeof(Guid) ||
                keyType == typeof(DateTimeOffset) || keyType == typeof(TimeSpan) ||
                keyType == typeof(nint) || keyType == typeof(nuint))
                IsJsonableKey = false;
            else
                IsJsonableKey = keyType.GetProperties().Length > 0;

            var basePath = $"{StartingPath}/{(string.IsNullOrWhiteSpace(Version) ? string.Empty : $"{Version}/")}{modelType.Name}/";
            GetPath = $"{basePath}{nameof(RepositoryMethods.Get)}?key={{0}}";
            ExistPath = $"{basePath}{nameof(RepositoryMethods.Exist)}?key={{0}}";
            DeletePath = $"{basePath}{nameof(RepositoryMethods.Delete)}?key={{0}}";
            QueryPath = $"{basePath}{nameof(RepositoryMethods.Query)}";
            OperationPath = $"{basePath}{nameof(RepositoryMethods.Operation)}?op={{0}}&returnType={{1}}";
            InsertPath = $"{basePath}{nameof(RepositoryMethods.Insert)}?key={{0}}";
            UpdatePath = $"{basePath}{nameof(RepositoryMethods.Update)}?key={{0}}";
            BatchPath = $"{basePath}{nameof(RepositoryMethods.Batch)}";
        }
    }
}
