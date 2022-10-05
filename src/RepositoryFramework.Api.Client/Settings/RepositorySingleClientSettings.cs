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
            IsJsonableKey = IKey.IsJsonable(keyType);

            var basePath = $"{StartingPath}/{(string.IsNullOrWhiteSpace(Version) ? string.Empty : $"{Version}/")}{modelType.Name}/";
            if (IsJsonableKey)
                GetPath = $"{basePath}{nameof(RepositoryMethods.Get)}";
            else
                GetPath = $"{basePath}{nameof(RepositoryMethods.Get)}?key={{0}}";
            if (IsJsonableKey)
                ExistPath = $"{basePath}{nameof(RepositoryMethods.Exist)}";
            else
                ExistPath = $"{basePath}{nameof(RepositoryMethods.Exist)}?key={{0}}";
            if (IsJsonableKey)
                DeletePath = $"{basePath}{nameof(RepositoryMethods.Delete)}";
            else
                DeletePath = $"{basePath}{nameof(RepositoryMethods.Delete)}?key={{0}}";
            QueryPath = $"{basePath}{nameof(RepositoryMethods.Query)}";
            OperationPath = $"{basePath}{nameof(RepositoryMethods.Operation)}?op={{0}}&returnType={{1}}";
            if (IsJsonableKey)
                InsertPath = $"{basePath}{nameof(RepositoryMethods.Insert)}";
            else
                InsertPath = $"{basePath}{nameof(RepositoryMethods.Insert)}?key={{0}}";
            if (IsJsonableKey)
                UpdatePath = $"{basePath}{nameof(RepositoryMethods.Update)}";
            else
                UpdatePath = $"{basePath}{nameof(RepositoryMethods.Update)}?key={{0}}";
            BatchPath = $"{basePath}{nameof(RepositoryMethods.Batch)}";
        }
    }
}
