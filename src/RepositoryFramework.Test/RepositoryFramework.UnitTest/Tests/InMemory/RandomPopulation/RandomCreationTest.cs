using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.InMemory.RandomPopulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.InMemory.RandomPopulation
{
    //https://github.com/moodmosaic/Fare/tree/master/Src/Fare
    public class RandomCreationTest
    {
        private static readonly IServiceProvider ServiceProvider;
        static RandomCreationTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
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
                .WithAutoIncrement(x => x.Id, 0)
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
                .AddRepositoryInMemoryStorage<AutoincrementModel, int>()
                .PopulateWithRandomData(x => x.Id)
                .WithAutoIncrement(x => x.Id, 0)
                .And()
                .AddRepositoryInMemoryStorage<AutoincrementModel2, int>()
                .PopulateWithRandomData(x => x.Id)
                .WithAutoIncrement(x => x.Id, 1)
                .Services
                .Finalize(out ServiceProvider)
                .Populate();
        }
        private readonly IRepository<PopulationTest, string> _test;
        private readonly IRepository<RegexPopulationTest, string> _population;
        private readonly IQueryPattern<DelegationPopulation, string> _delegation;
        private readonly IRepository<AutoincrementModel, int> _autoincrementRepository;
        private readonly IRepository<AutoincrementModel2, int> _autoincrementRepository2;

        public RandomCreationTest()
        {
            _test = ServiceProvider.GetService<IRepository<PopulationTest, string>>()!;
            _population = ServiceProvider.GetService<IRepository<RegexPopulationTest, string>>()!;
            _delegation = ServiceProvider.GetService<IQueryPattern<DelegationPopulation, string>>()!;
            _autoincrementRepository = ServiceProvider.GetService<IRepository<AutoincrementModel, int>>()!;
            _autoincrementRepository2 = ServiceProvider.GetService<IRepository<AutoincrementModel2, int>>()!;
        }
        [Fact]
        public async Task TestWithoutRegexAsync()
        {
            var all = await _test.QueryAsync().NoContext();
            var theFirst = all.First();
            Assert.NotEqual(0, theFirst.A);
            Assert.NotNull(theFirst.AA);
            Assert.NotEqual((uint)0, theFirst.B);
            Assert.NotNull(theFirst.BB);
            Assert.NotEqual((byte)0, theFirst.C);
            Assert.NotNull(theFirst.CC);
            Assert.NotEqual((sbyte)0, theFirst.D);
            Assert.NotNull(theFirst.DD);
            Assert.NotEqual((short)0, theFirst.E);
            Assert.NotNull(theFirst.EE);
            Assert.NotEqual((ushort)0, theFirst.F);
            Assert.NotNull(theFirst.FF);
            Assert.NotEqual((long)0, theFirst.G);
            Assert.NotNull(theFirst.GG);
            Assert.NotEqual((nint)0, theFirst.H);
            Assert.NotNull(theFirst.HH);
            Assert.NotEqual((nuint)0, theFirst.L);
            Assert.NotNull(theFirst.LL);
            Assert.NotEqual((float)0, theFirst.M);
            Assert.NotNull(theFirst.MM);
            Assert.NotEqual((double)0, theFirst.N);
            Assert.NotNull(theFirst.NN);
            Assert.NotEqual((decimal)0, theFirst.O);
            Assert.NotNull(theFirst.OO);
            Assert.NotNull(theFirst.P);
            Assert.NotNull(theFirst.PP);
            Assert.NotNull(theFirst.QQ);
            Assert.NotEqual((char)0, theFirst.R);
            Assert.NotNull(theFirst.RR);
            Assert.NotEqual(Guid.Empty, theFirst.S);
            Assert.NotNull(theFirst.SS);
            Assert.NotEqual(new DateTime(), theFirst.T);
            Assert.NotNull(theFirst.TT);
            Assert.NotEqual(TimeSpan.FromTicks(0), theFirst.U);
            Assert.NotNull(theFirst.UU);
            Assert.NotEqual(new DateTimeOffset(), theFirst.V);
            Assert.NotNull(theFirst.VV);
            Assert.NotEqual(new Range(), theFirst.Z);
            Assert.NotNull(theFirst.ZZ);
            Assert.NotNull(theFirst.X);
            Assert.Equal(10, theFirst?.X?.Count());
            Assert.NotNull(theFirst?.Y);
            Assert.Equal(10, theFirst?.Y?.Count);
            Assert.NotNull(theFirst?.W);
            Assert.Equal(10, theFirst?.W?.Length);
            Assert.NotNull(theFirst?.J);
            Assert.Equal(10, theFirst?.J?.Count);
            var regex = new Regex("[a-z]{4,5}");
            foreach (var check in theFirst!.J!)
            {
                Assert.Equal(check.A,
                    regex.Matches(check!.A!).OrderByDescending(x => x.Length).First().Value);
            }
            foreach (var check in theFirst!.Y!)
            {
                Assert.Equal(check.Value.A,
                    regex.Matches(check!.Value!.A!).OrderByDescending(x => x.Length).First().Value);
            }
            Assert.Equal(theFirst.I!.A!,
                    regex.Matches(theFirst.I!.A!).OrderByDescending(x => x.Length).First().Value);
            Assert.Equal(theFirst.II!.A!,
                    regex.Matches(theFirst.II!.A!).OrderByDescending(x => x.Length).First().Value);
        }
        [Fact]
        public async Task TestWithRegexAsync()
        {
            var all = await _population.OrderBy(x => x.Id).QueryAsync().NoContext();
            var theFirst = all.First();
            Assert.Equal(90, all.Count);
            Assert.Equal(0, all.First().Id);
            Assert.Equal(89, all.Last().Id);
            Assert.NotEqual(0, theFirst.A);
            Assert.NotNull(theFirst.AA);
            Assert.NotEqual((uint)0, theFirst.B);
            Assert.NotNull(theFirst.BB);
            Assert.NotEqual((byte)0, theFirst.C);
            Assert.NotNull(theFirst.CC);
            Assert.NotEqual((sbyte)0, theFirst.D);
            Assert.NotNull(theFirst.DD);
            Assert.NotEqual((short)0, theFirst.E);
            Assert.NotNull(theFirst.EE);
            Assert.NotEqual((ushort)0, theFirst.F);
            Assert.NotNull(theFirst.FF);
            Assert.NotEqual((long)0, theFirst.G);
            Assert.NotNull(theFirst.GG);
            Assert.NotEqual((nint)0, theFirst.H);
            Assert.NotNull(theFirst.HH);
            Assert.NotEqual((nuint)0, theFirst.L);
            Assert.NotNull(theFirst.LL);
            Assert.NotEqual((float)0, theFirst.M);
            Assert.NotNull(theFirst.MM);
            Assert.NotEqual((double)0, theFirst.N);
            Assert.NotNull(theFirst.NN);
            Assert.NotEqual((decimal)0, theFirst.O);
            Assert.NotNull(theFirst.OO);
            Assert.NotNull(theFirst.P);
            Assert.NotNull(theFirst.PP);
            Assert.NotNull(theFirst.QQ);
            Assert.NotEqual((char)0, theFirst.R);
            Assert.NotNull(theFirst.RR);
            Assert.NotEqual(Guid.Empty, theFirst.S);
            Assert.NotNull(theFirst.SS);
            Assert.NotEqual(new DateTime(), theFirst.T);
            Assert.NotNull(theFirst.TT);
            Assert.NotEqual(TimeSpan.FromTicks(0), theFirst.U);
            Assert.NotNull(theFirst.UU);
            Assert.NotEqual(new DateTimeOffset(), theFirst.V);
            Assert.NotNull(theFirst.VV);
            Assert.NotEqual(new Range(), theFirst.Z);
            Assert.NotNull(theFirst.ZZ);
            Assert.NotNull(theFirst.X);
            Assert.Equal(8, theFirst?.X?.Count());
            Assert.NotNull(theFirst?.Y);
            Assert.Equal(8, theFirst?.Y?.Count);
            Assert.NotNull(theFirst?.W);
            Assert.Equal(8, theFirst?.W?.Length);
            Assert.NotNull(theFirst?.J);
            Assert.Equal(8, theFirst?.J?.Count);
            var regex = new Regex("[a-z]{4,5}");
            foreach (var check in theFirst!.J!)
            {
                Assert.Equal(check.A,
                    regex.Matches(check!.A!).OrderByDescending(x => x.Length).First().Value);
            }
            foreach (var check in theFirst!.Y!)
            {
                Assert.Equal(check.Value.A,
                    regex.Matches(check!.Value!.A!).OrderByDescending(x => x.Length).First().Value);
            }
        }
        [Fact]
        public async Task TestWithDelegationAsync()
        {
            var all = await _delegation.QueryAsync().NoContext();
            var theFirst = all.First();
            Assert.Equal(2, theFirst.A);
            Assert.NotNull(theFirst.AA);
            Assert.Equal(2, theFirst.AA);
            Assert.Equal((uint)2, theFirst.B);
            Assert.NotNull(theFirst.BB);
            Assert.Equal((uint)2, theFirst.BB);
            Assert.Equal((byte)2, theFirst.C);
            Assert.NotNull(theFirst.CC);
            Assert.Equal((byte)2, theFirst.CC);
            Assert.Equal((sbyte)2, theFirst.D);
            Assert.NotNull(theFirst.DD);
            Assert.Equal((sbyte)2, theFirst.DD);
            Assert.Equal((short)2, theFirst.E);
            Assert.NotNull(theFirst.EE);
            Assert.Equal((short)2, theFirst.EE);
            Assert.Equal((ushort)2, theFirst.F);
            Assert.NotNull(theFirst.FF);
            Assert.Equal((ushort)2, theFirst.FF);
            Assert.Equal(2, theFirst.G);
            Assert.NotNull(theFirst.GG);
            Assert.Equal(2, theFirst.GG);
            Assert.Equal(2, theFirst.H);
            Assert.NotNull(theFirst.HH);
            Assert.Equal(2, theFirst.HH);
            Assert.Equal((nuint)2, theFirst.L);
            Assert.NotNull(theFirst.LL);
            Assert.Equal((nuint)2, theFirst.LL);
            Assert.Equal(2, theFirst.M);
            Assert.NotNull(theFirst.MM);
            Assert.Equal(2, theFirst.MM);
            Assert.Equal(2, theFirst.N);
            Assert.NotNull(theFirst.NN);
            Assert.Equal(2, theFirst.NN);
            Assert.Equal(2, theFirst.O);
            Assert.NotNull(theFirst.OO);
            Assert.Equal(2, theFirst.OO);
            Assert.NotNull(theFirst.P);
            Assert.NotNull(theFirst.PP);
            Assert.True(theFirst.Q);
            Assert.NotNull(theFirst.QQ);
            Assert.True(theFirst.QQ);
            Assert.Equal('a', theFirst.R);
            Assert.NotNull(theFirst.RR);
            Assert.Equal('a', theFirst.RR);
            Assert.NotEqual(Guid.Empty, theFirst.S);
            Assert.NotNull(theFirst.SS);
            Assert.NotEqual(new DateTime(), theFirst.T);
            Assert.NotNull(theFirst.TT);
            Assert.NotEqual(TimeSpan.FromTicks(0), theFirst.U);
            Assert.NotNull(theFirst.UU);
            Assert.NotEqual(new DateTimeOffset(), theFirst.V);
            Assert.NotNull(theFirst.VV);
            Assert.Equal(1, theFirst.Z.Start);
            Assert.Equal(2, theFirst.Z.End);
            Assert.NotNull(theFirst.ZZ);
            Assert.Equal(1, theFirst.ZZ?.Start);
            Assert.Equal(2, theFirst.ZZ?.End);
            Assert.NotNull(theFirst.X);
            Assert.Equal(10, theFirst?.X?.Count());
            Assert.NotNull(theFirst?.Y);
            Assert.Equal(10, theFirst?.Y?.Count);
            Assert.NotNull(theFirst?.W);
            Assert.Equal(10, theFirst?.W?.Length);
            Assert.NotNull(theFirst?.J);
            Assert.Equal(10, theFirst?.J?.Count);
            int counter = 0;
            foreach (var check in theFirst!.J!)
            {
                Assert.Equal(counter.ToString(), check.A);
                Assert.Equal(counter, check.B);
                counter++;
            }
        }
        [Fact]
        public async Task TestWithAutoincrementAsync()
        {
            var all = await _autoincrementRepository.OrderBy(x => x.Id).QueryAsync().NoContext();
            var all2 = await _autoincrementRepository2.OrderBy(x => x.Id).QueryAsync().NoContext();
            Assert.Equal(0, all.First().Id);
            Assert.Equal(99, all.Last().Id);
            Assert.Equal(1, all2.First().Id);
            Assert.Equal(100, all2.Last().Id);
        }
    }
}