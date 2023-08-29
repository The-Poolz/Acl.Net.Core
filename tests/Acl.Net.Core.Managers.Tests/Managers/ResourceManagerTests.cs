using Xunit;
using Acl.Net.Core.Database.Entities;
using Acl.Net.Core.Managers.Exceptions;
using Acl.Net.Core.Managers.Tests.Mock;

namespace Acl.Net.Core.Managers.Tests.Managers;

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
    public void IsPermitted_ShouldReturnPermittedResources_WhenGivenResourceNames()
    {
        var resourceNames = new[] { "PrivateResource", "PublicResource" };
        var resources = _resourceManager.IsPermitted(_adminUser, resourceNames).ToArray();

        Assert.Equal(2, resources.Length);
    }

    [Fact]
    public void IsPermitted_ShouldReturnEmpty_WhenGivenResourceNotAllowed()
    {
        var resourceNames = new[] { "PrivateResource" };
        var resources = _resourceManager.IsPermitted(_normalUser, resourceNames).ToArray();

        Assert.Empty(resources);
    }

    [Fact]
    public void IsPermitted_ShouldReturnPermittedResources_WhenGivenResources()
    {
        var resourcesList = new[] { _privateResource };
        var resources = _resourceManager.IsPermitted(_adminUser, resourcesList).ToArray();

        Assert.Single(resources);
        Assert.Equal(_privateResource, resources[0]);
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
    public async Task IsPermittedAsync_ShouldReturnPermittedResources_WhenGivenResourceNames()
    {
        Assert.Single(await _resourceManager.IsPermittedAsync(_adminUser, new[] { "PrivateResource" }));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnPermittedResources_WhenGivenResources()
    {
        Assert.Single(await _resourceManager.IsPermittedAsync(_adminUser, new[] { _privateResource }));
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
    public void GetResourceByName_ShouldThrowResourceNotFoundException_WhenResourceDoesNotExist()
    {
        const string nonExistentResourceName = "NonExistentResource";
        Assert.Throws<ResourceNotFoundException>(() => _resourceManager.GetResourceByName(nonExistentResourceName));
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
    public async Task GetResourceByNameAsync_ShouldThrowResourceNotFoundException_WhenResourceDoesNotExist()
    {
        const string nonExistentResourceName = "NonExistentResource";
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _resourceManager.GetResourceByNameAsync(nonExistentResourceName));
    }
}