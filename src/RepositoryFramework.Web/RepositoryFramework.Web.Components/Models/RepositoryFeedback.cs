namespace RepositoryFramework.Web.Components
{
    public sealed class RepositoryFeedback
    {
        public bool IsOk { get; set; }
        public string Title { get; set; }
        public string? Message { get; set; }
        public bool IsVisible { get; set; }
        public Func<ValueTask>? Ok { get; set; }
        public Func<ValueTask>? NotOk { get; set; }
        public bool HasCancelButton { get; set; }
    }
}
