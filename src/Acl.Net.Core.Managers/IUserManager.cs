using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Defines the contract for managing user-related operations using integer keys.
/// </summary>
public interface IUserManager : IUserManager<int>;

/// <summary>
/// Defines the contract for managing user-related operations with a specific key type.
/// </summary>
/// <typeparam name="TKey">
/// The type of the key, which must implement <see cref="IEquatable{TKey}"/>.
/// </typeparam>
public interface IUserManager<TKey> : IUserManager<TKey, User<TKey>, Role<TKey>>
    where TKey : IEquatable<TKey>;

/// <summary>
/// Defines the contract for managing user-related operations with specific user and role types.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type representing a user, which must inherit from <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TRole">The type representing a role, which must inherit from <see cref="Role{TKey}"/>.</typeparam>
public interface IUserManager<TKey, TUser, TRole>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
{
    public IEnumerable<TRole> GetUserRoles(string userName);

    public IEnumerable<TRole> GetUserRoles(TUser user);

    /// <summary>
    /// Retrieves an existing user by name or creates a new one with the specified role if it doesn't exist.
    /// </summary>
    /// <param name="userName">The name of the user to retrieve or create.</param>
    /// <param name="roleForNewUsers">The role to assign to the user if a new user is created.</param>
    /// <returns>The existing or newly created <see cref="TUser"/> instance.</returns>
    public TUser UserProcessing(string userName, TRole roleForNewUsers);

    /// <summary>
    /// Asynchronously retrieves an existing user by name or creates a new one with the specified role if it doesn't exist.
    /// </summary>
    /// <param name="userName">The name of the user to retrieve or create.</param>
    /// <param name="roleForNewUsers">The role to assign to the user if a new user is created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the existing or newly created <see cref="TUser"/> instance.</returns>
    public Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers);
}