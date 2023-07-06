using Xunit;
using Acl.Net.Core.Tests.Mock;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{
    private readonly AclManager aclManager;
    private readonly RoleDataSeeder initialDataSeeder;

    public AclManagerTests()
    {
        var context = InMemoryAclDbContext.CreateContext();
        aclManager = new AclManager(context);
        initialDataSeeder = new RoleDataSeeder();
    }

    [Fact]
    public void IsPermitted_StringParameters_NoMatchingResource_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => aclManager.IsPermitted("UserAccount", "NonExistentResource", initialDataSeeder));
    }

    [Fact]
    public void IsPermitted_StringParameters_ValidUserAndResource_ReturnsTrue()
    {
        var result = aclManager.IsPermitted("UserAccount", "PublicResource", initialDataSeeder);

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
    public void IsPermitted_ObjectParameters_UserWithoutAccess_ReturnsFalse()
    {
        var user = InMemoryAclDbContext.Users[0];
        var resource = InMemoryAclDbContext.Resources[1];

        var result = aclManager.IsPermitted(user, resource);

        Assert.False(result);
    }

    [Fact]
    public void IsPermitted_ObjectParameters_UserWithAccess_ReturnsTrue()
    {
        var user = InMemoryAclDbContext.Users[0];
        var resource = InMemoryAclDbContext.Resources[0];

        var result = aclManager.IsPermitted(user, resource);

        Assert.True(result);
    }
}
