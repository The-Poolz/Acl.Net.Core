using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Defines the basic contract for Access Control List (ACL) management.
/// </summary>
public interface IAclManager : IAclManager<int>
{
}

/// <summary>
/// Defines the contract for Access Control List (ACL) management with support for a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of key used to identify users and resources.</typeparam>
public interface IAclManager<TKey> : IAclManager<TKey, User<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
/// Defines the contract for Access Control List (ACL) management with support for specific key, user, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of key used to identify users and resources.</typeparam>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TResource">The type representing a resource.</typeparam>
public interface IAclManager<TKey, in TUser, TResource> : IDisposable
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TResource : Resource<TKey>
{
    /// <summary>
    /// Determines if a user is permitted to access a specific resource by name.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns><see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(string userName, string resourceName);

    /// <summary>
    /// Determines if a user is permitted to access a specific resource by name.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns><see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(TUser user, string resourceName);

    /// <summary>
    /// Determines if a user is permitted to access a specific resource.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="resource">The resource.</param>
    /// <returns><see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(string userName, TResource resource);

    /// <summary>
    /// Determines if a user is permitted to access a specific resource.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resource">The resource.</param>
    /// <returns><see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(TUser user, TResource resource);

    /// <summary>
    /// Determines if a user is permitted to access a collection of resources identified by names.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="resourceNames">The names of the resources.</param>
    /// <returns>A collection of resources that the user has access to.</returns>
    public IEnumerable<TResource> IsPermitted(string userName, IEnumerable<string> resourceNames);

    /// <summary>
    /// Determines asynchronously if a user is permitted to access a specific resource by name.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public Task<bool> IsPermittedAsync(string userName, string resourceName);

    /// <summary>
    /// Determines asynchronously if a user is permitted to access a specific resource by name.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public Task<bool> IsPermittedAsync(TUser user, string resourceName);

    /// <summary>
    /// Determines asynchronously if a user is permitted to access a specific resource.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public Task<bool> IsPermittedAsync(string userName, TResource resource);

    /// <summary>
    /// Determines asynchronously if a user is permitted to access a specific resource.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains <see langword="true"/> if access is permitted; otherwise, <see langword="false"/>.</returns>
    public Task<bool> IsPermittedAsync(TUser user, TResource resource);

    /// <summary>
    /// Determines asynchronously if a user is permitted to access a collection of resources identified by names.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="resourceNames">The names of the resources.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a collection of resources that the user has access to.</returns>
    public Task<IEnumerable<TResource>> IsPermittedAsync(string userName, IEnumerable<string> resourceNames);
}