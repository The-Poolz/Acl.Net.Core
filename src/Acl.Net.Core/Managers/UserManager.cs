using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Manages user-related operations using integer keys.
/// </summary>
public class UserManager : UserManager<int>, IUserManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserManager"/> class.
    /// </summary>
    /// <param name="context">The ACL database context.</param>
    public UserManager(AclDbContext context)
        : base(context)
    { }
}

/// <summary>
/// Manages user-related operations with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public class UserManager<TKey> : UserManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>, IUserManager<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserManager{TKey}"/> class.
    /// </summary>
    /// <param name="context">The ACL database context.</param>
    public UserManager(AclDbContext<TKey> context)
        : base(context)
    { }
}

/// <summary>
/// Manages user-related operations with specific user, role, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TUser">The type of the user.</typeparam>
/// <typeparam name="TRole">The type of the role.</typeparam>
/// <typeparam name="TResource">The type of the resource.</typeparam>
public class UserManager<TKey, TUser, TRole, TResource> : IUserManager<TKey, TUser, TRole>, IDisposable
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource> context;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserManager{TKey, TUser, TRole, TResource}"/> class.
    /// </summary>
    /// <param name="context">The ACL database context.</param>
    public UserManager(AclDbContext<TKey, TUser, TRole, TResource> context)
    {
        this.context = context;
    }

    /// <summary>
    /// Processes a user by either retrieving an existing user or creating a new one with the specified role.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="roleForNewUsers">The role for new users.</param>
    /// <returns>The retrieved or created user.</returns>
    public virtual TUser UserProcessing(string userName, TRole roleForNewUsers)
    {
        var user = context.Users.FirstOrDefault(x => x.Name == userName)
            ?? new TUser { Name = userName, RoleId = roleForNewUsers.Id };

        if (!user.Id.Equals(default)) return user;
        context.Users.Add(user);
        context.SaveChanges();

        return user;
    }

    /// <summary>
    /// Asynchronously processes a user by either retrieving an existing user or creating a new one with the specified role.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="roleForNewUsers">The role for new users.</param>
    /// <returns>A task that represents the asynchronous operation. The task result returns the retrieved or created user.</returns>
    public virtual async Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Name == userName)
            ?? new TUser { Name = userName, RoleId = roleForNewUsers.Id };

        if (!user.Id.Equals(default)) return user;
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;
        if (disposing)
        {
            context.Dispose();
        }
        isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
