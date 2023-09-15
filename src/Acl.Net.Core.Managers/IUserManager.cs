using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Defines the contract for managing user-related operations using integer keys.
/// </summary>
public interface IUserManager : IUserManager<int>
{
}

/// <summary>
/// Defines the contract for managing user-related operations with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IUserManager<TKey> : IUserManager<TKey, User<TKey>, Role<TKey>>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
/// Defines the contract for managing user-related operations with specific user and role types.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TUser">The type of the user.</typeparam>
/// <typeparam name="TRole">The type of the role.</typeparam>
public interface IUserManager<TKey, TUser, in TRole>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
{
    /// <summary>
    /// Processes a user by either retrieving an existing user or creating a new one with the specified role.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="roleForNewUsers">The role for new users.</param>
    /// <returns>The retrieved or created user.</returns>
    public TUser UserProcessing(string userName, TRole roleForNewUsers);

    /// <summary>
    /// Asynchronously processes a user by either retrieving an existing user or creating a new one with the specified role.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="roleForNewUsers">The role for new users.</param>
    /// <param name="cancellationToken">An optional token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result returns the retrieved or created user.</returns>
    public Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers, CancellationToken cancellationToken = default);
}