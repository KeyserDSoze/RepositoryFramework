using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.InMemory;
using RepositoryFramework.UnitTest.InMemory.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.InMemory.Exceptions
{
    public class Exceptions
    {
        private static readonly IServiceProvider ServiceProvider;
        static Exceptions()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                .AddRepositoryInMemoryStorage<Car, string>(options =>
                {
                    var customExceptions = new List<ExceptionOdds>
                    {
                        new ExceptionOdds()
                        {
                            Exception = new Exception("Normal Exception"),
                            Percentage = 10.352
                        },
                        new ExceptionOdds()
                        {
                            Exception = new Exception("Big Exception"),
                            Percentage = 49.1
                        },
                        new ExceptionOdds()
                        {
                            Exception = new Exception("Great Exception"),
                            Percentage = 40.548
                        }
                    };
                    options.AddForRepositoryPattern(new MethodBehaviorSetting
                    {
                        ExceptionOdds = customExceptions
                    });
                })
                .Services
                .Finalize(out ServiceProvider);
        }
        private readonly IRepository<Car, string> _car;
        public Exceptions()
        {
            _car = ServiceProvider.GetService<IRepository<Car, string>>()!;
        }
        [Fact]
        public async Task TestAsync()
        {
            try
            {
                var cars = await _car.QueryAsync().ToListAsync().NoContext();
                Assert.True(false);
                Assert.Empty(cars);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "Normal Exception" || ex.Message == "Big Exception" || ex.Message == "Great Exception");
            }
        }
    }
}