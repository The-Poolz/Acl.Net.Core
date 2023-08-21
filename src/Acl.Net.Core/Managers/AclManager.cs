using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Managers;

public class AclManager : AclManager<int>
{
    public AclManager(
        IUserManager userManager,
        IResourceManager resourceManager
    )
        : base(new RoleDataSeeder(), userManager, resourceManager)
    { }
}

public class AclManager<TKey> : AclManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
    public AclManager(
        IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder,
        IUserManager<TKey> userManager,
        IResourceManager<TKey> resourceManager
    )
        : base(initialDataSeeder, userManager, resourceManager)
    { }
}

public class AclManager<TKey, TUser, TRole, TResource> : IAclManager<TKey, TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly IInitialDataSeeder<TKey, TRole> initialDataSeeder;
    private readonly IUserManager<TKey, TUser, TRole> userManager;
    private readonly IResourceManager<TKey, TUser, TResource> resourceManager;

    public AclManager(
        IInitialDataSeeder<TKey, TRole> initialDataSeeder,
        IUserManager<TKey, TUser, TRole> userManager,
        IResourceManager<TKey, TUser, TResource> resourceManager
    )
    {
        this.initialDataSeeder = initialDataSeeder;
        this.userManager = userManager;
        this.resourceManager = resourceManager;
    }

    public virtual bool IsPermitted(string userName, string resourceName)
    {
        var user = userManager.UserProcessing(userName, initialDataSeeder.SeedUserRole());
        return resourceManager.IsPermitted(user, resourceName);
    }

    public virtual bool IsPermitted(TUser user, string resourceName)
    {
        return resourceManager.IsPermitted(user, resourceName);
    }

    public virtual bool IsPermitted(string userName, TResource resource)
    {
        var user = userManager.UserProcessing(userName, initialDataSeeder.SeedUserRole());
        return resourceManager.IsPermitted(user, resource);
    }

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return resourceManager.IsPermitted(user, resource);
    }

    public virtual async Task<bool> IsPermittedAsync(string userName, string resourceName)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        return await resourceManager.IsPermittedAsync(user, resourceName);
    }

    public virtual async Task<bool> IsPermittedAsync(TUser user, string resourceName)
    {
        return await resourceManager.IsPermittedAsync(user, resourceName);
    }

    public virtual async Task<bool> IsPermittedAsync(string userName, TResource resource)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        return await resourceManager.IsPermittedAsync(user, resource);
    }

    public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
    {
        return await resourceManager.IsPermittedAsync(user, resource);
    }
}
