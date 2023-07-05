using Xunit;
using Acl.Net.Core.Tests.Mock;
using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{
    private readonly AclManager aclManager;

    public AclManagerTests()
    {
        var context = InMemoryAclDbContext.CreateContext();
        aclManager = new AclManager(context);
    }

    [Fact]
    public void IsPermitted_StringParameters_NoMatchingClaim_ReturnsFalse()
    {
        var result = aclManager.IsPermitted("NonExistentToken", "publicResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_NoMatchingResource_ReturnsFalse()
    {
        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[0].Token, "NonExistentResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_UserWithoutAccess_ReturnsFalse()
    {
        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[0].Token, "privateResource");

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_StringParameters_UserWithAccess_ReturnsTrue()
    {
        var result = aclManager.IsPermitted(InMemoryAclDbContext.Claims[0].Token, "publicResource");

        Assert.True(result);
    }

    [Fact]
    public void IsPermitted_ObjectParameters_UserWithoutAccess_ReturnsFalse()
    {
        var result = aclManager.IsPermitted(InMemoryAclDbContext.Users[0], InMemoryAclDbContext.Resources[1]);

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_ObjectParameters_UserWithAccess_ReturnsTrue()
    {
        var result = aclManager.IsPermitted(InMemoryAclDbContext.Users[0], InMemoryAclDbContext.Resources[0]);

        Assert.True(result);
    }
}
