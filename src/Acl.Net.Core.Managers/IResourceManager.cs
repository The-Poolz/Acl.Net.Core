using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Manages resource-related operations with a specific integer key type.
/// </summary>
public interface IResourceManager : IResourceManager<int>
{
}

/// <summary>
/// Manages resource-related operations with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IResourceManager<TKey> : IResourceManager<TKey, User<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
/// Manages resource-related operations with specific user and resource types.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TUser">The type of the user.</typeparam>
/// <typeparam name="TResource">The type of the resource.</typeparam>
public interface IResourceManager<TKey, in TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TResource : Resource<TKey>
{
    /// <summary>
    /// Determines whether the user is permitted to access the specified resource by its name.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(TUser user, string resourceName);

    /// <summary>
    /// Determines whether the user is permitted to access the specified resource.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resource">The resource.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(TUser user, TResource resource);

    /// <summary>
    /// Check which resources are allowed for user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resourceNames">Resources that will be checked for the user.</param>
    /// <returns>Returns the allowed resources for the user.</returns>
    public IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<string> resourceNames);

    /// <summary>
    /// Check which resources are allowed for user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Returns the allowed resources for the user.</returns>
    public IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<TResource> resources);

    /// <summary>
    /// Asynchronously determines whether the user is permitted to access the specified resource by its name.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result is <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public Task<bool> IsPermittedAsync(TUser user, string resourceName);

    /// <summary>
    /// Asynchronously determines whether the user is permitted to access the specified resource.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result is <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public Task<bool> IsPermittedAsync(TUser user, TResource resource);

    /// <summary>
    /// Asynchronously checks which resources are allowed for a user by their names.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resourceNames">Resources that will be checked for the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result returns the allowed resources for the user.</returns>
    public Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<string> resourceNames);

    /// <summary>
    /// Asynchronously checks which resources are allowed for a user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result returns the allowed resources for the user.</returns>
    public Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<TResource> resources);

    /// <summary>
    /// Retrieves a resource by its name.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>The resource with the specified name.</returns>
    public TResource GetResourceByName(string resourceName);

    /// <summary>
    /// Retrieves a resource by its name asynchronously.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the resource with the specified name.</returns>
    public Task<TResource> GetResourceByNameAsync(string resourceName);
}