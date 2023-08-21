using Acl.Net.Core.Entities;
using Acl.Net.Core.Exceptions;
using Acl.Net.Core.Extensions;
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

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id) ||
               context.Resources.Any(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    /// <summary>
    /// Check if the user is permitted for at least one resource in resources.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Return <see langword="true" /> if at least one resource is allowed to the user, otherwise <see langword="false" />.</returns>
    public virtual bool IsPermitted(TUser user, IEnumerable<TResource> resources)
    {
        return user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id) ||
               resources.Any(resource => context.Resources.Any(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id)));
    }

    public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
    {
        return user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id) ||
               await context.Resources.AnyAsync(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    /// <summary>
    /// Check if the user is permitted for at least one resource in resources.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Return <see langword="true" /> if at least one resource is allowed to the user, otherwise <see langword="false" />.</returns>
    public virtual async Task<bool> IsPermittedAsync(TUser user, IEnumerable<TResource> resources)
    {
        if (user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id)) return true;

        return await resources.AnyAsync(async resource =>
            await context.Resources.AnyAsync(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id)));
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

    public virtual IEnumerable<TResource> GetResourcesByName(string resourceName)
    {
        return context.Resources.Where(r => r.Name == resourceName)
            ?? throw new ResourceNotFoundException(resourceName);
    }
}
