using Acl.Net.Core.Entities;
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
    public void Constructor_ValidParameters_DoesNotThrowException()
    {
        var context = InMemoryAclDbContext.CreateContext();

        var manager = new AclManager(context, new SecretsProvider());

        Assert.IsType<AclManager>(manager);
    }

    [Fact]
    public void IsPermitted_StringParameters_NoMatchingResource_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => aclManager.IsPermitted("UserAccount", "NonExistentResource"));
    }

    [Fact]
    public void IsPermitted_StringParameters_ValidUserAndResource_ReturnsTrue()
    {
        var result = aclManager.IsPermitted("UserAccount", "PublicResource");
        Assert.True(result);
    }

    [Fact]
    public void UserProcessing_ExistingUser_ReturnsUser()
    {
        var user = aclManager.UserProcessing("UserAccount");
        Assert.Equal("UserAccount", user.Name);
    }

    [Fact]
    public void UserProcessing_NonExistingUserWithoutRoleName_ReturnsUserWithoutRole()
    {
        var user = aclManager.UserProcessing("NonExistingUser");
        Assert.Equal("NonExistingUser", user.Name);
        Assert.Equal(default, user.RoleId);
    }

    [Fact]
    public void UserProcessing_NonExistingUserWithInvalidRoleName_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => aclManager.UserProcessing("NonExistingUser", "NonExistentRole"));
    }

    [Fact]
    public void UserProcessing_NonExistingUserWithRoleName_AddsUserAndReturnsUser()
    {
        var user = aclManager.UserProcessing("NonExistingUser", "UserRole");
        Assert.Equal("NonExistingUser", user.Name);
        Assert.Equal(1, user.RoleId);
    }

    [Fact]
    public void ClaimProcessing_UserWithoutClaim_GeneratesNewClaim()
    {
        var user = new User { Id = 3, Name = "UserWithoutClaim", RoleId = 1 };
        var resource = InMemoryAclDbContext.Resources[0];
        var claim = aclManager.ClaimProcessing(user, resource);
        Assert.NotNull(claim);
        Assert.Equal(3, claim.UserId);
    }

    [Fact]
    public void ClaimProcessing_UserWithExpiredClaim_GeneratesNewClaim()
    {
        var user = InMemoryAclDbContext.Users[0];
        var resource = InMemoryAclDbContext.Resources[0];
        var claim = aclManager.ClaimProcessing(user, resource);
        Assert.NotNull(claim);
        Assert.Equal(user.Id, claim.UserId);
    }

    [Fact]
    public void ClaimProcessing_UserWithValidClaim_ReturnsExistingClaim()
    {
        var user = InMemoryAclDbContext.Users[1];
        var resource = InMemoryAclDbContext.Resources[0];
        var claim = aclManager.ClaimProcessing(user, resource);
        Assert.NotNull(claim);
        Assert.Equal(user.Id, claim.UserId);
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
