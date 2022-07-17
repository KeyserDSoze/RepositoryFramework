using RepositoryFramework.UnitTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class MethodsTest
    {
        private readonly IRepository<User> _user1;
        private readonly IRepository<SuperUser, string> _user2;
        private readonly IRepository<IperUser, string, State> _user3;
        public MethodsTest(IRepository<User> user1,
            IRepository<SuperUser, string> user2,
            IRepository<IperUser, string, State> user3)
        {
            _user1 = user1;
            _user2 = user2;
            _user3 = user3;
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
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
                case 3:
                    users = await _user3.QueryAsync().NoContext();
                    break;
            }
            Assert.Equal(100, users.Count());
        }
    }
}