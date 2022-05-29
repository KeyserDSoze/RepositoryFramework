﻿namespace RepositoryFramework.Client
{
    internal class IntRepositoryClient<T> : RepositoryClient<T, int>, IIntableRepositoryClient<T>, IIntableQueryClient<T>, IIntableCommandClient<T>, IRepositoryClient<T, int>, IQueryClient<T, int>, ICommandClient<T, int>
    {
        public IntRepositoryClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}