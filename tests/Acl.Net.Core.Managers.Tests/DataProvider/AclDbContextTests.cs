using Xunit;
using Acl.Net.Core.Database;

namespace Acl.Net.Core.Managers.Tests.DataProvider;

public class AclDbContextTests
{
    [Fact]
    public void FirstConstructorTest()
    {
        var context = new AclDbContext();

        Assert.NotNull(context);
    }
}