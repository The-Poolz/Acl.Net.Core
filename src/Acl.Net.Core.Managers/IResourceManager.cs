using Acl.Net.Core.Database.Entities;
using Acl.Net.Core.Managers.Exceptions;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Defines the contract for managing resource-related operations with a specific integer key type.
/// </summary>
public interface IResourceManager : IResourceManager<int>;

/// <summary>
/// Defines the contract for managing resource-related operations with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IResourceManager<TKey> : IResourceManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>;

/// <summary>
/// Defines the contract for managing resource-related operations with specific user and resource types.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type representing a user, which must inherit from <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TRole">The type representing a role, which must inherit from <see cref="Role{TKey}"/>.</typeparam>
/// <typeparam name="TResource">The type representing a resource, which must inherit from <see cref="Resource{TKey}"/>.</typeparam>
public interface IResourceManager<TKey, in TUser, in TRole, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    /// <summary>
    /// Determines whether the specified role is permitted to access the resource identified by its name.
    /// </summary>
    /// <param name="role">The role to check permissions for.</param>
    /// <param name="resourceName">The name of the resource to check.</param>
    /// <returns>
    /// <see langword="true"/> if the role is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource name does not exist.</exception>
    public bool IsPermitted(TRole role, string resourceName);

    /// <summary>
    /// Determines whether the specified role is permitted to access the given resource.
    /// </summary>
    /// <param name="role">The role to check permissions for.</param>
    /// <param name="resource">The resource to check.</param>
    /// <returns>
    /// <see langword="true"/> if the role is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsPermitted(TRole role, TResource resource);

    /// <summary>
    /// Determines whether the specified user is permitted to access the resource identified by its name.
    /// </summary>
    /// <param name="user">The user to check permissions for.</param>
    /// <param name="resourceName">The name of the resource to check.</param>
    /// <returns>
    /// <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource name does not exist.</exception>
    public bool IsPermitted(TUser user, string resourceName);

    /// <summary>
    /// Determines whether the specified user is permitted to access the given resource.
    /// </summary>
    /// <param name="user">The user to check permissions for.</param>
    /// <param name="resource">The resource to check.</param>
    /// <returns>
    /// <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsPermitted(TUser user, TResource resource);

    /// <summary>
    /// Determines which resources from a collection of resource names the specified user is permitted to access.
    /// </summary>
    /// <param name="user">The user to check permissions for.</param>
    /// <param name="resourceNames">The collection of resource names to check.</param>
    /// <returns>
    /// An enumerable of <see cref="TResource"/> that the user is permitted to access.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource name does not exist.</exception>
    public IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<string> resourceNames);

    /// <summary>
    /// Determines which resources from a collection the specified user is permitted to access.
    /// </summary>
    /// <param name="user">The user to check permissions for.</param>
    /// <param name="resources">The collection of resources to check.</param>
    /// <returns>
    /// An enumerable of <see cref="TResource"/> that the user is permitted to access.
    /// </returns>
    public IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<TResource> resources);

    /// <summary>
    /// Retrieves a resource by its name.
    /// </summary>
    /// <param name="resourceName">The name of the resource to retrieve.</param>
    /// <returns>
    /// The <see cref="TResource"/> with the specified name.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource name does not exist.</exception>
    public TResource GetResourceByName(string resourceName);
}
