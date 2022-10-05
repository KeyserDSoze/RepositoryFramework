using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace RepositoryFramework.UnitTest.Tests.Api
{
    internal sealed class HttpClientFactory : IHttpClientFactory
    {
        public static HttpClientFactory Instance { get; } = new HttpClientFactory();
        public IHost? Host { get; set; }
        public IServiceProvider? ServiceProvider { get; set; }
        private HttpClientFactory() { }
        public HttpClient CreateClient(string name)
            => Host!.GetTestServer().CreateClient();
        public async Task<HttpClient> StartAsync()
        {
            await Host!.StartAsync();
            var server = Host.GetTestServer();
            return server.CreateClient();
        }
    }
}
