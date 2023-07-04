using Xunit;
using Acl.Net.Core.Tests.Mock;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{
    [Fact]
    public void IsPermitted_StringParameters_NoMatchingClaim_ReturnsFalse()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(context);

        var result = aclManager.IsPermitted("NonExistentToken", "publicResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_NoMatchingUser_ReturnsFalse()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(context);

        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[2].Token, "publicResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_NoMatchingResource_ReturnsFalse()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(context);

        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[0].Token, "NonExistentResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_UserWithoutAccess_ReturnsFalse()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(context);

        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[0].Token, "privateResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_UserWithAccess_ReturnsTrue()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(context);

        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[0].Token, "publicResource");

        Assert.True(result);
    }
}