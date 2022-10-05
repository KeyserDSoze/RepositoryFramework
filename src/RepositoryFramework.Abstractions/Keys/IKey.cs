namespace RepositoryFramework
{
    public interface IKey
    {
        string AsString();
        internal static string GetStringedValues(params object[] inputs)
            => string.Join('-', inputs.Select(x => x.ToString()));
        public static bool IsJsonable(Type keyType)
        {
            if (keyType == typeof(string) || keyType == typeof(Guid) ||
                keyType == typeof(DateTimeOffset) || keyType == typeof(TimeSpan) ||
                keyType == typeof(nint) || keyType == typeof(nuint))
                return false;
            else
                return keyType.GetProperties().Length > 0;
        }
    }
}
