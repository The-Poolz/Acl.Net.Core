using Xunit;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Tests.DataProvider;

public class AclDbContextTests
{
    [Fact]
    public void FirstConstructorTest()
    {
        var context = new AclDbContext();

        Assert.NotNull(context);
    }
}