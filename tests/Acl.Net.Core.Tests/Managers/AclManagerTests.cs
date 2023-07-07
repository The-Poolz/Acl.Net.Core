using Xunit;
using Acl.Net.Core.Managers;
using Acl.Net.Core.Tests.Mock;

namespace Acl.Net.Core.Tests.Managers;

public class AclManagerTests
{
    private readonly AclManager _aclManager;

    public AclManagerTests()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var userManager = new UserManager(context);
        var resourceManager = new ResourceManager(context);
        _aclManager = new AclManager(userManager, resourceManager);
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
    public async Task IsPermittedAsync_ShouldReturnTrue_WhenAdminAccessPrivateResource()
    {
        Assert.True(await _aclManager.IsPermittedAsync("AdminAccount", "PrivateResource"));
    }

    [Fact]
    public async Task IsPermittedAsync_ShouldReturnFalse_WhenUserAccessPrivateResource()
    {
        Assert.False(await _aclManager.IsPermittedAsync("UserAccount", "PrivateResource"));
    }
}
