using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Models
{
    public class IperUser : SuperUser { }
    public class SuperUser : User { }
    public class User
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
        public IDictionary<string, string>? Keys { get; set; }
        public int[]? Indexes { get; set; }
    }
    public class Claim
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
