using RepositoryFramework.UnitTest.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    //https://github.com/moodmosaic/Fare/tree/master/Src/Fare
    public class RandomCreationTest
    {
        private readonly IRepository<PopulationTest, string> _test;
        private readonly IStringableRepository<RegexPopulationTest> _population;
        private readonly IStringableQuery<DelegationPopulation> _delegation;

        public RandomCreationTest(IRepository<PopulationTest, string> test,
            IStringableRepository<RegexPopulationTest> population,
            IStringableQuery<DelegationPopulation> delegation)
        {
            _test = test;
            _population = population;
            _delegation = delegation;
        }
        [Fact]
        public async Task TestWithoutRegexAsync()
        {
            var all = await _test.QueryAsync();
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
            Assert.NotEqual(new Guid("00000000-0000-0000-0000-000000000000"), theFirst.S);
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
        }
        [Fact]
        public async Task TestWithRegexAsync()
        {
            var all = await _population.QueryAsync();
            var theFirst = all.First();
            Assert.Equal(90, all.Count());
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
            Assert.NotEqual(new Guid("00000000-0000-0000-0000-000000000000"), theFirst.S);
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
            var all = await _delegation.QueryAsync();
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
            Assert.NotEqual(new Guid("00000000-0000-0000-0000-000000000000"), theFirst.S);
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
    }
}