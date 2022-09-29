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
        private static readonly IServiceProvider s_serviceProvider;
        static PopulationTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                 .AddRepositoryInMemoryStorage<User, string>(options =>
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
                .Finalize(out s_serviceProvider)
                .Populate();
        }
        private readonly IRepository<User, string> _user1;
        private readonly IRepository<SuperUser, string> _user2;
        public PopulationTest()
        {
            _user1 = s_serviceProvider.GetService<IRepository<User, string>>()!;
            _user2 = s_serviceProvider.GetService<IRepository<SuperUser, string>>()!;
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task TestAsync(int numberOfParameters)
        {
            switch (numberOfParameters)
            {
                case 1:
                    var users = await _user1.QueryAsync().ToListAsync().NoContext();
                    Assert.Equal(100, users.Count);
                    break;
                case 2:
                    var users2 = await _user2.QueryAsync().ToListAsync().NoContext();
                    Assert.Equal(100, users2.Count);
                    break;
            }
        }
    }
}
