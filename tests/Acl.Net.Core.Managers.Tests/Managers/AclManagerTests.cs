using Xunit;
using Acl.Net.Core.Database;
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
    public void Ctor_WithManagersAndInitialDataSeederParameters()
    {
        var context = InMemoryAclDbContext.CreateContext();
        var aclManager = new AclManager(new RoleDataSeeder(), new UserManager(context), new ResourceManager(context));

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
}
