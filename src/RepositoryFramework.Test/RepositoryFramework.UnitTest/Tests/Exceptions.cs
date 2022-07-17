using RepositoryFramework.UnitTest.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class Exceptions
    {
        private readonly IRepositoryPattern<Car, string> _car;
        public Exceptions(IRepositoryPattern<Car, string> car)
        {
            _car = car;
        }
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "Test")]
        public async Task TestAsync()
        {
            try
            {
                var cars = await _car.QueryAsync().NoContext();
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message == "Normal Exception" || ex.Message == "Big Exception" || ex.Message == "Great Exception");
            }
        }
    }
}