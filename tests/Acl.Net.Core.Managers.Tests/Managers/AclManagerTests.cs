using Xunit;
using FluentAssertions;
using Acl.Net.Core.Database;
using Acl.Net.Core.Managers.Tests.Mock;

namespace Acl.Net.Core.Managers.Tests.Managers;

public class AclManagerTests
{
    public class Constructors
    {
        [Fact]
        public void WithContextParameter()
        {
            var context = InMemoryAclDbContext.CreateContext();
            var aclManager = new AclManager(context);

            Assert.NotNull(aclManager);
        }

        [Fact]
        public void CtorWithManagersAndInitialDataSeederParameters()
        {
            var context = InMemoryAclDbContext.CreateContext();
            var aclManager = new AclManager(new RoleDataSeeder(), context);

            Assert.NotNull(aclManager);
        }
    }

    public class IsPermitted
    {
        private readonly IAclManager _aclManager = new AclManager(InMemoryAclDbContext.CreateContext());

        [Theory]
        [InlineData("AdminAccount", true, false, "NotExistResource")]
        [InlineData("AdminAccount", true, false, "PrivateResource")]
        [InlineData("AdminAccount", true, false, "PublicResource")]
        [InlineData("UserAccount", false, true, "NotExistResource")]
        [InlineData("UserAccount", false, false, "PrivateResource")]
        [InlineData("UserAccount", true, false, "PublicResource")]
        [InlineData("NotExistAccount", false, true, "PublicResource")]
        public void Test(string userName, bool expected, bool withError, string resourceName)
        {
            try
            {
                _aclManager.IsPermitted(userName, resourceName).Should().Be(expected);
            }
            catch (Exception)
            {
                withError.Should().BeTrue();
            }
        }
    }
}
