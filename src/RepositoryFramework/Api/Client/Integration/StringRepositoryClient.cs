﻿namespace RepositoryFramework.Client
{
    internal class StringRepositoryClient<T> : RepositoryClient<T, string>, IStringableRepositoryClient<T>, IStringableQueryClient<T>, IStringableCommandClient<T>, IRepositoryClient<T, string>, IQueryClient<T, string>, ICommandClient<T, string>
    {
        public StringRepositoryClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}