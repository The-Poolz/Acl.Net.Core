using Xunit;
using Acl.Net.Core.Entities;
using Acl.Net.Core.Managers;
using Acl.Net.Core.Tests.Mock;

namespace Acl.Net.Core.Tests.Managers;

public class ResourceManagerTests
{
    private readonly ResourceManager _resourceManager;
    private readonly User _adminUser;
    private readonly User _normalUser;
    private readonly Resource _privateResource;

    public ResourceManagerTests()
    {
        _resourceManager = new ResourceManager(InMemoryAclDbContext.CreateContext());

        _adminUser = new User { Id = 2, Name = "AdminAccount", RoleId = 1 };
        _normalUser = new User { Id = 1, Name = "UserAccount", RoleId = 2 };
        _privateResource = new Resource { Id = 2, Name = "PrivateResource", RoleId = 1 };
    }

    [Fact]
    public void IsPermitted_ShouldReturnTrue_WhenAdminAccessPrivateResource()
    {
        Assert.True(_resourceManager.IsPermitted(_adminUser, _privateResource));
    }

    [Fact]
    public void IsPermitted_ShouldReturnFalse_WhenUserAccessPrivateResource()
    {
        Assert.False(_resourceManager.IsPermitted(_normalUser, _privateResource));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnTrue_WhenAdminAccessPrivateResource()
    {
        Assert.True(await _resourceManager.IsPermittedAsync(_adminUser, _privateResource));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnFalse_WhenUserAccessPrivateResource()
    {
        Assert.False(await _resourceManager.IsPermittedAsync(_normalUser, _privateResource));
    }

    [Fact]
    public void GetResourceByName_ShouldReturnResource_WhenResourceExists()
    {
        const string expectedResourceName = "PrivateResource";
        var resource = _resourceManager.GetResourceByName(expectedResourceName);
        Assert.NotNull(resource);
        Assert.Equal(expectedResourceName, resource.Name);
    }

    [Fact]
    public void GetResourceByName_ShouldThrowInvalidOperationException_WhenResourceDoesNotExist()
    {
        const string nonExistentResourceName = "NonExistentResource";
        Assert.Throws<InvalidOperationException>(() => _resourceManager.GetResourceByName(nonExistentResourceName));
    }

    [Fact]
    public async Task GetResourceByNameAsync_ShouldReturnResource_WhenResourceExists()
    {
        const string expectedResourceName = "PrivateResource";
        var resource = await _resourceManager.GetResourceByNameAsync(expectedResourceName);
        Assert.NotNull(resource);
        Assert.Equal(expectedResourceName, resource.Name);
    }

    [Fact]
    public async Task GetResourceByNameAsync_ShouldThrowInvalidOperationException_WhenResourceDoesNotExist()
    {
        const string nonExistentResourceName = "NonExistentResource";
        await Assert.ThrowsAsync<InvalidOperationException>(() => _resourceManager.GetResourceByNameAsync(nonExistentResourceName));
    }
}