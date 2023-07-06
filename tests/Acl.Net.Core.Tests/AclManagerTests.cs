using Xunit;
using Acl.Net.Core.Tests.Mock;
using Acl.Net.Core.Tests.Cryptography;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{
    private readonly AclManager aclManager;

    public AclManagerTests()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");
        var context = InMemoryAclDbContext.CreateContext();
        aclManager = new AclManager(context, new SecretsProvider());
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
