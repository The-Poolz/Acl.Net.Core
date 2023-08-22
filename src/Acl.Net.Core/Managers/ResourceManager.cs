using Acl.Net.Core.Entities;
using Acl.Net.Core.Exceptions;
using Acl.Net.Core.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.Managers;

public class ResourceManager : ResourceManager<int>, IResourceManager
{
    public ResourceManager(AclDbContext context)
        : base(context, new RoleDataSeeder())
    { }
}

public class ResourceManager<TKey> : ResourceManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>, IResourceManager<TKey>
    where TKey : IEquatable<TKey>
{
    public ResourceManager(
        AclDbContext<TKey> context,
        IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder
    )
        : base(context, initialDataSeeder)
    { }
}

public class ResourceManager<TKey, TUser, TRole, TResource> : IResourceManager<TKey, TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource> context;
    private readonly IInitialDataSeeder<TKey, TRole> initialDataSeeder;

    public ResourceManager(
        AclDbContext<TKey, TUser, TRole, TResource> context,
        IInitialDataSeeder<TKey, TRole> initialDataSeeder
    )
    {
        this.context = context;
        this.initialDataSeeder = initialDataSeeder;
    }

    public virtual bool IsPermitted(TUser user, string resourceName)
    {
        var resource = GetResourceByName(resourceName);
        return IsPermitted(user, resource);
    }

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id) ||
            context.Resources.Any(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    public virtual IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<string> resourceNames)
    {
        return resourceNames.Select(GetResourceByName).Where(resource => IsPermitted(user, resource));
    }

    public virtual IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<TResource> resources)
    {
        return resources.Where(resource => IsPermitted(user, resource));
    }
    
    public virtual async Task<bool> IsPermittedAsync(TUser user, string resourceName)
    {
        var resource = await GetResourceByNameAsync(resourceName);
        return await IsPermittedAsync(user, resource);
    }

    public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
    {
        return user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id) ||
            await context.Resources.AnyAsync(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    public virtual async Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<string> resourceNames)
    {
        var permittedResources = new List<TResource>();
        foreach (var resourceName in resourceNames)
        {
            var resource = await GetResourceByNameAsync(resourceName);
            if (await IsPermittedAsync(user, resource))
            {
                permittedResources.Add(resource);
            }
        }
        return permittedResources.AsEnumerable();
    }

    public virtual async Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<TResource> resources)
    {
        var permittedResources = new List<TResource>();
        foreach (var resource in resources)
        {
            if (await IsPermittedAsync(user, resource))
            {
                permittedResources.Add(resource);
            }
        }
        return permittedResources.AsEnumerable();
    }

    public virtual TResource GetResourceByName(string resourceName)
    {
        return context.Resources.FirstOrDefault(r => r.Name == resourceName)
            ?? throw new ResourceNotFoundException(resourceName);
    }

    public virtual async Task<TResource> GetResourceByNameAsync(string resourceName)
    {
        return await context.Resources.FirstOrDefaultAsync(r => r.Name == resourceName)
            ?? throw new ResourceNotFoundException(resourceName);
    }
}
