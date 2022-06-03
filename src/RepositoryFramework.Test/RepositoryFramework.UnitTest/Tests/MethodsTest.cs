using RepositoryFramework.UnitTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class MethodsTest
    {
        private readonly IRepositoryPattern<User, string> _user;
        public MethodsTest(IRepositoryPattern<User, string> user)
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