using Acl.Net.Core.Database.Entities;
using Acl.Net.Core.Managers.Exceptions;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Defines the basic contract for Access Control List (ACL) management.
/// </summary>
public interface IAclManager : IAclManager<int>;

/// <summary>
/// Defines the contract for Access Control List (ACL) management with support for a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of key used to identify users and resources.</typeparam>
public interface IAclManager<TKey> : IAclManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>;

/// <summary>
/// Defines the contract for Access Control List (ACL) management with support for specific key, user, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type representing a user, which must inherit from <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TRole">The type representing a role, which must inherit from <see cref="Role{TKey}"/>.</typeparam>
/// <typeparam name="TResource">The type representing a resource, which must inherit from <see cref="Resource{TKey}"/>.</typeparam>
public interface IAclManager<in TKey, in TUser, in TRole, in TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole: Role<TKey>
    where TResource : Resource<TKey>
{
    /// <summary>
    /// Determines if the specified user by name is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns><see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource by name does not exist.</exception>
    /// <exception cref="UserNotFoundException">Thrown when the specified user by name does not exist.</exception>
    public bool IsPermitted(string userName, string resourceName);

    /// <summary>
    /// Determines if the specified user object is permitted to access the specified resource object.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns> <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(TUser user, TResource resource);

    /// <summary>
    /// Determines whether the specified role is permitted to access the given resource.
    /// </summary>
    /// <param name="role">The role to check permissions for.</param>
    /// <param name="resource">The resource to check.</param>
    /// <returns><see langword="true"/> if the role is permitted to access the resource; otherwise, <see langword="false"/>.</returns>
    public bool IsPermitted(TRole role, TResource resource);
}