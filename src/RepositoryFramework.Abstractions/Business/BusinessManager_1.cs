namespace RepositoryFramework
{
    internal sealed class BusinessManager<T> : BusinessManager<T, string>, IBusinessManager<T>
    {
        public BusinessManager(IServiceProvider serviceProvider, BusinessManagerOptions<T, string> options) : base(serviceProvider, options)
        {
        }
    }

}
