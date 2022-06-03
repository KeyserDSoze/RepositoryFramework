using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.Models;
using RepositoryFramework.UnitTest.Storage;
using Rystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryFramework.UnitTest
{
    public class Startup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "It needed for DI")]
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            services.AddSingleton(configuration);
            services
                .AddRystem()
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
                .AddRepositoryInMemoryStorage<PopulationTest, string>()
                .PopulateWithRandomData(x => x.P)
                .WithPattern(x => x.J!.First().A, "[a-z]{4,5}")
                .WithPattern(x => x.Y!.First().Value.A, "[a-z]{4,5}")
                .WithImplementation(x => x.I, typeof(MyInnerInterfaceImplementation))
                .WithPattern(x => x.I!.A!, "[a-z]{4,5}")
                .WithPattern(x => x.II!.A!, "[a-z]{4,5}")
                .WithImplementation<IInnerInterface, MyInnerInterfaceImplementation>(x => x.I!)
                .And()
                .AddRepositoryInMemoryStorage<RegexPopulationTest, string>()
                .PopulateWithRandomData(x => x.P, 90, 8)
                .WithPattern(x => x.A, "[1-9]{1,2}")
                .WithPattern(x => x.AA, "[1-9]{1,2}")
                .WithPattern(x => x.B, "[1-9]{1,2}")
                .WithPattern(x => x.BB, "[1-9]{1,2}")
                .WithPattern(x => x.C, "[1-9]{1,2}")
                .WithPattern(x => x.CC, "[1-9]{1,2}")
                .WithPattern(x => x.D, "[1-9]{1,2}")
                .WithPattern(x => x.DD, "[1-9]{1,2}")
                .WithPattern(x => x.E, "[1-9]{1,2}")
                .WithPattern(x => x.EE, "[1-9]{1,2}")
                .WithPattern(x => x.F, "[1-9]{1,2}")
                .WithPattern(x => x.FF, "[1-9]{1,2}")
                .WithPattern(x => x.G, "[1-9]{1,2}")
                .WithPattern(x => x.GG, "[1-9]{1,2}")
                .WithPattern(x => x.H, "[1-9]{1,3}")
                .WithPattern(x => x.HH, "[1-9]{1,3}")
                .WithPattern(x => x.L, "[1-9]{1,3}")
                .WithPattern(x => x.LL, "[1-9]{1,3}")
                .WithPattern(x => x.M, "[1-9]{1,2}")
                .WithPattern(x => x.MM, "[1-9]{1,2}")
                .WithPattern(x => x.N, "[1-9]{1,2}")
                .WithPattern(x => x.NN, "[1-9]{1,2}")
                .WithPattern(x => x.O, "[1-9]{1,2}")
                .WithPattern(x => x.OO, "[1-9]{1,2}")
                .WithPattern(x => x.P, "[1-9a-zA-Z]{5,20}")
                .WithPattern(x => x.Q, "true")
                .WithPattern(x => x.QQ, "true")
                .WithPattern(x => x.R, "[a-z]{1}")
                .WithPattern(x => x.RR, "[a-z]{1}")
                .WithPattern(x => x.S, "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})")
                .WithPattern(x => x.SS, "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})")
                .WithPattern(x => x.T, @"(?:2018|2019|2020|2021|2022)/(?:10|11|12)/(?:06|07|08) (00:00:00)")
                .WithPattern(x => x.TT, @"(?:2018|2019|2020|2021|2022)/(?:10|11|12)/(?:06|07|08) (00:00:00)")
                .WithPattern(x => x.U, "[1-9]{1,4}")
                .WithPattern(x => x.UU, "[1-9]{1,4}")
                .WithPattern(x => x.V, @"(?:10|11|12)/(?:06|07|08)/(?:2018|2019|2020|2021|2022) (00:00:00 AM \+00:00)")
                .WithPattern(x => x.VV, @"(?:10|11|12)/(?:06|07|08)/(?:2018|2019|2020|2021|2022) (00:00:00 AM \+00:00)")
                .WithPattern(x => x.Z, "[1-9]{1,2}", "[1-9]{1,2}")
                .WithPattern(x => x.ZZ, "[1-9]{1,2}", "[1-9]{1,2}")
                .WithPattern(x => x.J!.First().A, "[a-z]{4,5}")
                .WithPattern(x => x.Y!.First().Value.A, "[a-z]{4,5}")
                .And()
                .AddRepositoryInMemoryStorage<DelegationPopulation, string>()
                .PopulateWithRandomData(x => x.P)
                .WithValue(x => x.A, () => 2)
                .WithValue(x => x.AA, () => 2)
                .WithValue(x => x.B, () => (uint)2)
                .WithValue(x => x.BB, () => (uint)2)
                .WithValue(x => x.C, () => (byte)2)
                .WithValue(x => x.CC, () => (byte)2)
                .WithValue(x => x.D, () => (sbyte)2)
                .WithValue(x => x.DD, () => (sbyte)2)
                .WithValue(x => x.E, () => (short)2)
                .WithValue(x => x.EE, () => (short)2)
                .WithValue(x => x.F, () => (ushort)2)
                .WithValue(x => x.FF, () => (ushort)2)
                .WithValue(x => x.G, () => 2)
                .WithValue(x => x.GG, () => 2)
                .WithValue(x => x.H, () => (nint)2)
                .WithValue(x => x.HH, () => (nint)2)
                .WithValue(x => x.L, () => (nuint)2)
                .WithValue(x => x.LL, () => (nuint)2)
                .WithValue(x => x.M, () => 2)
                .WithValue(x => x.MM, () => 2)
                .WithValue(x => x.N, () => 2)
                .WithValue(x => x.NN, () => 2)
                .WithValue(x => x.O, () => 2)
                .WithValue(x => x.OO, () => 2)
                .WithValue(x => x.P, () => Guid.NewGuid().ToString())
                .WithValue(x => x.Q, () => true)
                .WithValue(x => x.QQ, () => true)
                .WithValue(x => x.R, () => 'a')
                .WithValue(x => x.RR, () => 'a')
                .WithValue(x => x.S, () => Guid.NewGuid())
                .WithValue(x => x.SS, () => Guid.NewGuid())
                .WithValue(x => x.T, () => DateTime.UtcNow)
                .WithValue(x => x.TT, () => DateTime.UtcNow)
                .WithValue(x => x.U, () => new TimeSpan(2))
                .WithValue(x => x.UU, () => new TimeSpan(2))
                .WithValue(x => x.V, () => DateTimeOffset.UtcNow)
                .WithValue(x => x.VV, () => DateTimeOffset.UtcNow)
                .WithValue(x => x.Z, () => new Range(new Index(1), new Index(2)))
                .WithValue(x => x.ZZ, () => new Range(new Index(1), new Index(2)))
                .WithValue(x => x.J, () =>
                {
                    List<InnerDelegationPopulation> inners = new();
                    for (int i = 0; i < 10; i++)
                    {
                        inners.Add(new InnerDelegationPopulation()
                        {
                            A = i.ToString(),
                            B = i
                        });
                    }
                    return inners;
                })
                .And()
                .Finalize()
                .AddMigration<MigrationUser, string, MigrationFrom, MigrationTo>(x => x.NumberOfConcurrentInserts = 2)
                .FinalizeWithoutDependencyInjection();
            ServiceLocator.GetService<IServiceProvider>().Populate();
        }
    }
}
