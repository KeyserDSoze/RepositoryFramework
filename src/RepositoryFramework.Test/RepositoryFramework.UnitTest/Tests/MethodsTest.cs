using RepositoryFramework.UnitTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class MethodsTest
    {
        private readonly IRepository<User, string> _user;
        public MethodsTest(IRepository<User, string> user)
        {
            _user = user;
        }
        [Fact]
        public async Task TestAsync()
        {
            var users = await _user.QueryAsync();
            Assert.True(users.Count() == 100);
        }
    }
}