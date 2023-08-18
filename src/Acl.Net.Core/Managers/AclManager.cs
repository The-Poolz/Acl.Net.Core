﻿using Acl.Net.Core.Entities;
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

public class AclManager<TKey, TUser, TRole, TResource> : IAclManager
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
        var resource = resourceManager.GetResourceByName(resourceName);
        return resourceManager.IsPermitted(user, resource);
    }

    public virtual async Task<bool> IsPermittedAsync(string userName, string resourceName)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        var resource = await resourceManager.GetResourceByNameAsync(resourceName);
        return await resourceManager.IsPermittedAsync(user, resource);
    }
}
