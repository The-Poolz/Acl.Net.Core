using Acl.Net.Core.Database;
using Microsoft.EntityFrameworkCore;
using Acl.Net.Core.Database.Entities;
using Acl.Net.Core.Managers.Exceptions;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Manages resource-related operations with a specific integer key type.
/// </summary>
public class ResourceManager : ResourceManager<int>, IResourceManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceManager"/> class with the specified context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ResourceManager(AclDbContext context)
        : base(context, new RoleDataSeeder())
    { }
}

/// <summary>
/// Manages resource-related operations with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public class ResourceManager<TKey> : ResourceManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>, IResourceManager<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceManager{TKey}"/> class with the specified context and initial data seeder.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="initialDataSeeder">The initial data seeder.</param>
    public ResourceManager(
        AclDbContext<TKey> context,
        IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder
    )
        : base(context, initialDataSeeder)
    { }
}

/// <summary>
/// Manages resource-related operations with specific user, role, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TUser">The type of the user.</typeparam>
/// <typeparam name="TRole">The type of the role.</typeparam>
/// <typeparam name="TResource">The type of the resource.</typeparam>
public class ResourceManager<TKey, TUser, TRole, TResource> : IResourceManager<TKey, TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    protected readonly AclDbContext<TKey, TUser, TRole, TResource> Context;
    protected readonly IInitialDataSeeder<TKey, TRole> InitialDataSeeder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceManager{TKey, TUser, TRole, TResource}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="initialDataSeeder">The initial data seeder for roles.</param>
    public ResourceManager(
        AclDbContext<TKey, TUser, TRole, TResource> context,
        IInitialDataSeeder<TKey, TRole> initialDataSeeder
    )
    {
        Context = context;
        InitialDataSeeder = initialDataSeeder;
    }

    /// <summary>
    /// Checks if a user has permission to a specific resource by its name.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual bool IsPermitted(TUser user, string resourceName)
    {
        var resource = GetResourceByName(resourceName);
        return IsPermitted(user, resource);
    }

    /// <summary>
    /// Checks if a user has permission to a specific resource.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resource">The resource to check.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return user.RoleId.Equals(InitialDataSeeder.SeedAdminRole().Id) ||
            Context.Resources.Any(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    /// <summary>
    /// Filters resources by names that a user is permitted to access.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resourceNames">The names of the resources to check.</param>
    /// <returns>An enumerable of resources that the user is permitted to access.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when one or more of the specified resource names do not exist.
    /// </exception>
    public virtual IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<string> resourceNames)
    {
        return resourceNames.Select(GetResourceByName).Where(resource => IsPermitted(user, resource));
    }

    /// <summary>
    /// Filters resources that a user is permitted to access.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resources">The resources to check.</param>
    /// <returns>An enumerable of resources that the user is permitted to access.</returns>
    public virtual IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<TResource> resources)
    {
        return resources.Where(resource => IsPermitted(user, resource));
    }

    /// <summary>
    /// Asynchronously checks if a user has permission to a specific resource by its name.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual async Task<bool> IsPermittedAsync(TUser user, string resourceName)
    {
        var resource = await GetResourceByNameAsync(resourceName);
        return await IsPermittedAsync(user, resource);
    }

    /// <summary>
    /// Asynchronously checks if a user has permission to a specific resource.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resource">The resource to check.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
    {
        return user.RoleId.Equals(InitialDataSeeder.SeedAdminRole().Id) ||
            await Context.Resources.AnyAsync(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    /// <summary>
    /// Asynchronously filters resources by names that a user is permitted to access.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resourceNames">The names of the resources to check.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains an enumerable of resources that the user is permitted to access.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when one or more of the specified resource names do not exist.
    /// </exception>
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

    /// <summary>
    /// Asynchronously filters resources that a user is permitted to access.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="resources">The resources to check.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains an enumerable of resources that the user is permitted to access.</returns>
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

    /// <summary>
    /// Retrieves a resource by its name.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>The resource with the specified name, or throws <see cref="ResourceNotFoundException"/> if the resource is not found.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual TResource GetResourceByName(string resourceName)
    {
        return Context.Resources.FirstOrDefault(r => r.Name == resourceName)
            ?? throw new ResourceNotFoundException(resourceName);
    }

    /// <summary>
    /// Retrieves a resource by its name asynchronously.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the resource with the specified name, or throws <see cref="ResourceNotFoundException"/> if the resource is not found.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual async Task<TResource> GetResourceByNameAsync(string resourceName)
    {
        return await Context.Resources.FirstOrDefaultAsync(r => r.Name == resourceName)
            ?? throw new ResourceNotFoundException(resourceName);
    }
}
