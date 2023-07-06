using Xunit;
using Acl.Net.Core.Tests.Mock;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{
    private readonly AclManager aclManager;

    public AclManagerTests()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");
        var context = InMemoryAclDbContext.CreateContext();
        aclManager = new AclManager(context);
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
