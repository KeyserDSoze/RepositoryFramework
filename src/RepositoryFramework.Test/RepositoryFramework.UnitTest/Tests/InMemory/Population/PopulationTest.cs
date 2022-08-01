using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.InMemory;
using RepositoryFramework.UnitTest.InMemory.Population.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.InMemory.Population
{
    public class PopulationTest
    {
        private static readonly IServiceProvider ServiceProvider;
        static PopulationTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                 .AddRepositoryInMemoryStorage<User>(options =>
                 {
                     var writingRange = new Range(int.Parse(configuration["data_creation:delay_in_write_from"]),
                         int.Parse(configuration["data_creation:delay_in_write_to"]));
                     options.AddForCommandPattern(new MethodBehaviorSetting
                     {
                         MillisecondsOfWait = writingRange,
                     });
                     var readingRange = new Range(int.Parse(configuration["data_creation:delay_in_read_from"]),
                         int.Parse(configuration["data_creation:delay_in_read_to"]));
                     options.AddForQueryPattern(new MethodBehaviorSetting
                     {
                         MillisecondsOfWait = readingRange
                     });
                 })
                .PopulateWithRandomData(x => x.Id!, 100)
                .WithPattern(x => x.Email, @"[a-z]{4,10}@gmail\.com")
                .And()
                .AddRepositoryInMemoryStorage<SuperUser, string>(options =>
                {
                    var writingRange = new Range(int.Parse(configuration["data_creation:delay_in_write_from"]),
                        int.Parse(configuration["data_creation:delay_in_write_to"]));
                    options.AddForCommandPattern(new MethodBehaviorSetting
                    {
                        MillisecondsOfWait = writingRange,
                    });
                    var readingRange = new Range(int.Parse(configuration["data_creation:delay_in_read_from"]),
                        int.Parse(configuration["data_creation:delay_in_read_to"]));
                    options.AddForQueryPattern(new MethodBehaviorSetting
                    {
                        MillisecondsOfWait = readingRange
                    });
                })
                .PopulateWithRandomData(x => x.Id!, 100)
                .WithPattern(x => x.Email, @"[a-z]{4,10}@gmail\.com")
                .And()
                .Services
                .Finalize(out ServiceProvider)
                .Populate();
        }
        private readonly IRepository<User> _user1;
        private readonly IRepository<SuperUser, string> _user2;
        public PopulationTest()
        {
            _user1 = ServiceProvider.GetService<IRepository<User>>()!;
            _user2 = ServiceProvider.GetService<IRepository<SuperUser, string>>()!;
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task TestAsync(int numberOfParameters)
        {
            IEnumerable<User> users = null!;
            switch (numberOfParameters)
            {
                case 1:
                    users = await _user1.QueryAsync().NoContext();
                    break;
                case 2:
                    users = await _user2.QueryAsync().NoContext();
                    break;
            }
            Assert.Equal(100, users.Count());
        }
    }
}