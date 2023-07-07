using Xunit;
using Acl.Net.Core.Tests.Mock;
using Acl.Net.Core.DataProvider;
using Acl.Net.Core.Managers;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{
    private readonly AclManager aclManager;
    private readonly RoleDataSeeder initialDataSeeder;

    public AclManagerTests()
    {
        var context = InMemoryAclDbContext.CreateContext();
        initialDataSeeder = new RoleDataSeeder();
        aclManager = new AclManager(context);
    }

    [Fact]
    public void IsPermitted_StringParameters_UserNameIsNullOrEmpty_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => aclManager.IsPermitted("    ", "NonExistentResource"));
    }

    [Fact]
    public void IsPermitted_StringParameters_ResourceNameIsNullOrEmpty_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => aclManager.IsPermitted("UserAccount", "    "));
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
        var role = initialDataSeeder.SeedUserRole();
        var user = aclManager.UserProcessing("UserAccount", role);

        Assert.Equal("UserAccount", user.Name);
        Assert.Equal(role.Id, user.RoleId);
    }

    [Fact]
    public void UserProcessing_NonExistingUser_AddsUserAndReturnsUser()
    {
        var role = initialDataSeeder.SeedUserRole();
        var user = aclManager.UserProcessing("NonExistingUser", role);

        Assert.Equal("NonExistingUser", user.Name);
        Assert.Equal(role.Id, user.RoleId);
    }

    [Fact]
    public void IsPermitted_ObjectParameters_UserIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => aclManager.IsPermitted(null!, InMemoryAclDbContext.Resources[1]));
    }

    [Fact]
    public void IsPermitted_ObjectParameters_ResourceIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => aclManager.IsPermitted(InMemoryAclDbContext.Users[0], null!));
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

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void IsPermitted_ObjectParameters_AdminCall_ReturnsTrue(int index)
    {
        var result = aclManager.IsPermitted(InMemoryAclDbContext.Users[1], InMemoryAclDbContext.Resources[index]);

        Assert.True(result);
    }
}
