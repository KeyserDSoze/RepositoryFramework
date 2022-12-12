namespace RepositoryFramework.Web.Components
{
    public sealed class AppMenuSettings
    {
        public int Index { get; set; }
        /// <summary>
        /// Select icon from https://fonts.google.com/icons?selected=Material+Icons&icon.style=Outlined
        /// </summary>
        public string Icon { get; set; } = "hexagon";
        public string Name { get; set; }
        public required string QueryUri { get; init; }
        public required string CreateUri { get; init; }
        public required Type KeyType { get; init; }
        public required Type ModelType { get; init; }
        public static AppMenuSettings CreateDefault(Type modelType, Type keyType)
        {
            return new AppMenuSettings
            {
                KeyType = keyType,
                ModelType = modelType,
                Name = modelType.Name,
                CreateUri = $"../../../../Repository/{modelType.Name}/Create",
                QueryUri = $"../../../../Repository/{modelType.Name}/Query",
                Index = int.MaxValue
            };
        }
    }
}
