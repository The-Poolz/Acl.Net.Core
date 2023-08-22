using Acl.Net.Core.Entities;
using Acl.Net.Core.Exceptions;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Manages access control lists (ACLs) using integer keys.
/// This class provides a simplified interface for managing ACLs with integer keys, by extending the more generic AclManager with TKey.
/// </summary>
public class AclManager : AclManager<int>, IAclManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager"/> class with the provided user manager and resource manager.
    /// </summary>
    /// <param name="userManager">An implementation of <see cref="IUserManager"/>.</param>
    /// <param name="resourceManager">An implementation of <see cref="IResourceManager"/>.</param>
    public AclManager(
        IUserManager userManager,
        IResourceManager resourceManager
    )
        : base(new RoleDataSeeder(), userManager, resourceManager)
    { }
}

/// <summary>
/// Manages access control lists (ACLs) using keys of type TKey.
/// This class provides the base functionality for managing ACLs with specified key types.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
public class AclManager<TKey> : AclManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>, IAclManager<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager{TKey}"/> class with the provided initial data seeder, user manager, and resource manager.
    /// </summary>
    /// <param name="initialDataSeeder">An implementation of <see cref="IInitialDataSeeder{TKey, TRole}"/> used to seed initial role data.</param>
    /// <param name="userManager">An implementation of <see cref="IUserManager{TKey}"/>.</param>
    /// <param name="resourceManager">An implementation of <see cref="IResourceManager{TKey}"/>.</param>
    public AclManager(
        IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder,
        IUserManager<TKey> userManager,
        IResourceManager<TKey> resourceManager
    )
        : base(initialDataSeeder, userManager, resourceManager)
    { }
}

/// <summary>
/// Manages access control lists (ACLs) using keys, users, roles, and resources of specified types.
/// This class provides the complete functionality for managing ACLs with specified key, user, role, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type of the user, which must be a derived type of <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TRole">The type of the role, which must be a derived type of <see cref="Role{TKey}"/></typeparam>
/// <typeparam name="TResource">The type of the resource, which must be a derived type of <see cref="Resource{TKey}"/></typeparam>
public class AclManager<TKey, TUser, TRole, TResource> : IAclManager<TKey, TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly IInitialDataSeeder<TKey, TRole> initialDataSeeder;
    private readonly IUserManager<TKey, TUser, TRole> userManager;
    private readonly IResourceManager<TKey, TUser, TResource> resourceManager;
    protected bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager{TKey, TUser, TRole, TResource}"/> class with the provided initial data seeder, user manager, and resource manager.
    /// </summary>
    /// <param name="initialDataSeeder">An implementation of <see cref="IInitialDataSeeder{TKey, TRole}"/> used to seed initial role data.</param>
    /// <param name="userManager">An implementation of <see cref="IUserManager{TKey, TUser, TRole}"/> for user management.</param>
    /// <param name="resourceManager">An implementation of <see cref="IResourceManager{TKey, TUser, TResource}"/> for resource management.</param>
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

    /// <summary>
    /// Determines if the specified user name is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual bool IsPermitted(string userName, string resourceName)
    {
        var user = userManager.UserProcessing(userName, initialDataSeeder.SeedUserRole());
        return resourceManager.IsPermitted(user, resourceName);
    }
    
    /// <summary>
    /// Determines if the specified user object is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual bool IsPermitted(TUser user, string resourceName)
    {
        return resourceManager.IsPermitted(user, resourceName);
    }

    /// <summary>
    /// Determines if the specified user name is permitted to access the specified resource object.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public virtual bool IsPermitted(string userName, TResource resource)
    {
        var user = userManager.UserProcessing(userName, initialDataSeeder.SeedUserRole());
        return resourceManager.IsPermitted(user, resource);
    }

    /// <summary>
    /// Determines if the specified user object is permitted to access the specified resource object.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return resourceManager.IsPermitted(user, resource);
    }

    /// <summary>
    /// Determines the resources that the specified user name is permitted to access from a collection of resource names.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceNames">The collection of resource names to check permissions against.</param>
    /// <returns>A collection of <see cref="TResource"/> objects that the user is permitted to access;
    /// an empty collection if the user is not permitted to access any of the resources.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when one or more of the specified resource names do not exist.
    /// </exception>
    public virtual IEnumerable<TResource> IsPermitted(string userName, IEnumerable<string> resourceNames)
    {
        var user = userManager.UserProcessing(userName, initialDataSeeder.SeedUserRole());
        return resourceManager.IsPermitted(user, resourceNames);
    }

    /// <summary>
    /// Determines asynchronously if the specified user name is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual async Task<bool> IsPermittedAsync(string userName, string resourceName)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        return await resourceManager.IsPermittedAsync(user, resourceName);
    }

    /// <summary>
    /// Determines asynchronously if the specified user object is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified resource name does not exist.
    /// </exception>
    public virtual async Task<bool> IsPermittedAsync(TUser user, string resourceName)
    {
        return await resourceManager.IsPermittedAsync(user, resourceName);
    }

    /// <summary>
    /// Determines asynchronously if the specified user name is permitted to access the specified resource object.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public virtual async Task<bool> IsPermittedAsync(string userName, TResource resource)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        return await resourceManager.IsPermittedAsync(user, resource);
    }

    /// <summary>
    /// Determines asynchronously if the specified user object is permitted to access the specified resource object.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
    {
        return await resourceManager.IsPermittedAsync(user, resource);
    }

    /// <summary>
    /// Determines asynchronously the resources that the specified user name is permitted to access from a collection of resource names.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceNames">The collection of resource names to check permissions against.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a collection of <see cref="TResource"/> objects that the user is permitted to access;
    /// an empty collection if the user is not permitted to access any of the resources.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when one or more of the specified resource names do not exist.
    /// </exception>
    public virtual async Task<IEnumerable<TResource>> IsPermittedAsync(string userName, IEnumerable<string> resourceNames)
    {
        var user = await userManager.UserProcessingAsync(userName, initialDataSeeder.SeedUserRole());
        return await resourceManager.IsPermittedAsync(user, resourceNames);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="AclManager{TKey, TUser, TRole, TResource}"/> class and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to release both managed and unmanaged resources; 
    /// <see langword="false"/> to release only unmanaged resources.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this method is called after the object has already been disposed of.
    /// </exception>
    protected virtual void DisposeManagers(bool disposing)
    {
        if (isDisposed) throw new ObjectDisposedException(nameof(AclManager<TKey, TUser, TRole, TResource>));
        if (disposing)
        {
            userManager.Dispose();
            resourceManager.Dispose();
        }
        isDisposed = true;
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="AclManager{TKey, TUser, TRole, TResource}"/> class.<br/>
    /// This method calls <see cref="DisposeManagers(bool)"/> with <see langword="true"/> and then suppresses finalization.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this method is called after the object has already been disposed of.
    /// </exception>
    public void Dispose()
    {
        DisposeManagers(disposing: true);
        GC.SuppressFinalize(this);
    }
}
