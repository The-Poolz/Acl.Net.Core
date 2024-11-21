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
public interface IAclManager<TKey> : IAclManager<TKey, User<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>;

/// <summary>
/// Defines the contract for Access Control List (ACL) management with support for specific key, user, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type representing a user, which must inherit from <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TResource">The type representing a resource, which must inherit from <see cref="Resource{TKey}"/>.</typeparam>
public interface IAclManager<TKey, in TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TResource : Resource<TKey>
{
    /// <summary>
    /// Determines if the specified user by name is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns>
    /// <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource name does not exist.</exception>
    //public bool IsPermitted(string userName, string resourceName);

    /// <summary>
    /// Determines if the specified user object is permitted to access the specified resource by name.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resourceName">The name of the resource to check permission against.</param>
    /// <returns>
    /// <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the specified resource name does not exist.</exception>
    //public bool IsPermitted(TUser user, string resourceName);

    /// <summary>
    /// Determines if the specified user by name is permitted to access the specified resource object.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns>
    /// <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    //public bool IsPermitted(string userName, TResource resource);

    /// <summary>
    /// Determines if the specified user object is permitted to access the specified resource object.
    /// </summary>
    /// <param name="user">The user object to check permission for.</param>
    /// <param name="resource">The resource object to check permission against.</param>
    /// <returns>
    /// <see langword="true"/> if the user is permitted to access the resource; otherwise, <see langword="false"/>.
    /// </returns>
    //public bool IsPermitted(TUser user, TResource resource);

    /// <summary>
    /// Determines the resources that the specified user by name is permitted to access from a collection of resource names.
    /// </summary>
    /// <param name="userName">The name of the user to check permission for.</param>
    /// <param name="resourceNames">The collection of resource names to check permissions against.</param>
    /// <returns>
    /// A collection of <see cref="TResource"/> objects that the user is permitted to access;
    /// an empty collection if the user is not permitted to access any of the resources.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">Thrown when one or more of the specified resource names do not exist.</exception>
    //public IEnumerable<TResource> IsPermitted(string userName, IEnumerable<string> resourceNames);
}