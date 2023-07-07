using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Acl.Net.Core.Managers;

public class AclManager : AclManager<int>
{
    public AclManager(AclDbContext context)
        : base(context, new RoleDataSeeder())
    { }
}

public class AclManager<TKey> : AclManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
    public AclManager(AclDbContext<TKey, User<TKey>, Role<TKey>, Resource<TKey>> context, IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder)
        : base(context, initialDataSeeder)
    { }
}

public class AclManager<TKey, TUser, TRole, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly IInitialDataSeeder<TKey, TRole> initialDataSeeder;
    private readonly UserManager<TKey, TUser, TRole, TResource> userManager;
    private readonly ResourceManager<TKey, TUser, TRole, TResource> resourceManager;

    public AclManager(IServiceProvider serviceProvider)
    {
        AclDbContext<TKey, TUser, TRole, TResource> context = serviceProvider.GetRequiredService<AclDbContext<TKey, TUser, TRole, TResource>>();
        initialDataSeeder = serviceProvider.GetRequiredService<IInitialDataSeeder<TKey, TRole>>();

        userManager = serviceProvider.GetService<UserManager<TKey, TUser, TRole, TResource>>()
            ?? new UserManager<TKey, TUser, TRole, TResource>(context);
        resourceManager = serviceProvider.GetService<ResourceManager<TKey, TUser, TRole, TResource>>()
            ?? new ResourceManager<TKey, TUser, TRole, TResource>(context, initialDataSeeder);
    }

    public bool IsPermitted(string userName, string resourceName)
    {
        var user = userManager.UserProcessing(userName, initialDataSeeder.SeedUserRole());
        var resource = resourceManager.GetResourceByName(resourceName);
        return resourceManager.IsPermitted(user, resource);
    }

    public async Task<bool> IsPermittedAsync(string userName, string resourceName)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        var resource = await resourceManager.GetResourceByNameAsync(resourceName);
        return await resourceManager.IsPermittedAsync(user, resource);
    }
}
