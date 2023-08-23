using Xunit;
using FluentAssertions;
using Acl.Net.Core.Managers.Tests.Mock;

namespace Acl.Net.Core.Managers.Tests.Managers;

public class AclManagerTests
{
    private readonly AclManager _aclManager;

    public AclManagerTests()
    {
        var context = InMemoryAclDbContext.CreateContext();
        _aclManager = new AclManager(context);
    }

    [Fact]
    public void Ctor_WithContextParameter()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(context);

        Assert.NotNull(aclManager);
    }

    [Fact]
    public void Ctor_WithManagersParameters()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(new UserManager(context), new ResourceManager(context));

        Assert.NotNull(aclManager);
    }

    [Fact]
    public void IsPermitted_ShouldReturnTrue_WhenAdminAccessPrivateResource()
    {
        Assert.True(_aclManager.IsPermitted("AdminAccount", "PrivateResource"));
    }

    [Fact]
    public void IsPermitted_ShouldReturnFalse_WhenUserAccessPrivateResource()
    {
        Assert.False(_aclManager.IsPermitted("UserAccount", "PrivateResource"));
    }

    [Fact]
    public void IsPermitted_ShouldReturnPermittedResources_WhenGivenResourceNames()
    {
        Assert.Single(_aclManager.IsPermitted("AdminAccount", new[] { "PrivateResource" }));
    }

    [Fact]
    public void IsPermitted_ShouldReturnTrue_WhenAdminAccessPrivateResourceByUserObject()
    {
        var adminUser = InMemoryAclDbContext.AdminAccount;
        Assert.True(_aclManager.IsPermitted(adminUser, "PrivateResource"));
    }

    [Fact]
    public void IsPermitted_ShouldReturnFalse_WhenUserAccessPrivateResourceByUserObject()
    {
        var user = InMemoryAclDbContext.UserAccount;
        Assert.False(_aclManager.IsPermitted(user, "PrivateResource"));
    }

    [Fact]
    public void IsPermitted_ShouldReturnTrue_WhenAdminAccessPrivateResourceByResourceObject()
    {
        var privateResource = InMemoryAclDbContext.PrivateResource;
        Assert.True(_aclManager.IsPermitted("AdminAccount", privateResource));
    }

    [Fact]
    public void IsPermitted_ShouldReturnFalse_WhenUserAccessPrivateResourceByResourceObject()
    {
        var privateResource = InMemoryAclDbContext.PrivateResource;
        Assert.False(_aclManager.IsPermitted("UserAccount", privateResource));
    }

    [Fact]
    public void IsPermitted_ShouldReturnFalse_WhenUserAccessPrivateResourceByResourceObjectAndUserObject()
    {
        var privateResource = InMemoryAclDbContext.PrivateResource;
        var user = InMemoryAclDbContext.UserAccount;
        Assert.False(_aclManager.IsPermitted(user, privateResource));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnPermittedResources_WhenGivenResourceNames()
    {
        Assert.Single(await _aclManager.IsPermittedAsync("AdminAccount", new[] { "PrivateResource" }));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnFalse_WhenUserAccessPrivateResource()
    {
        Assert.False(await _aclManager.IsPermittedAsync("UserAccount", "PrivateResource"));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnTrue_WhenAdminAccessPrivateResource()
    {
        Assert.True(await _aclManager.IsPermittedAsync("AdminAccount", "PrivateResource"));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnTrue_WhenAdminAccessPrivateResourceByUserObject()
    {
        var adminUser = InMemoryAclDbContext.AdminAccount;
        Assert.True(await _aclManager.IsPermittedAsync(adminUser, "PrivateResource"));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnFalse_WhenUserAccessPrivateResourceByUserObject()
    {
        var user = InMemoryAclDbContext.UserAccount;
        Assert.False(await _aclManager.IsPermittedAsync(user, "PrivateResource"));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnTrue_WhenAdminAccessPrivateResourceByResourceObject()
    {
        var privateResource = InMemoryAclDbContext.PrivateResource;
        Assert.True(await _aclManager.IsPermittedAsync("AdminAccount", privateResource));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnFalse_WhenUserAccessPrivateResourceByResourceObject()
    {
        var privateResource = InMemoryAclDbContext.PrivateResource;
        Assert.False(await _aclManager.IsPermittedAsync("UserAccount", privateResource));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnFalse_WhenUserAccessPrivateResourceByResourceObjectAndUserObject()
    {
        var privateResource = InMemoryAclDbContext.PrivateResource;
        var user = InMemoryAclDbContext.UserAccount;
        Assert.False(await _aclManager.IsPermittedAsync(user, privateResource));
    }

    [Fact]
    public void Dispose_ShouldNotThrowException_WhenCalledOnce()
    {
        var action = () => _aclManager.Dispose();
        action.Should().NotThrow();
    }

    [Fact]
    public void Dispose_ShouldThrowObjectDisposedException_WhenCalledTwice()
    {
        _aclManager.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _aclManager.Dispose());
    }
}
