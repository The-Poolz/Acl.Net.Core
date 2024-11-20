using Acl.Net.Core.Database;
using Microsoft.EntityFrameworkCore;
using Acl.Net.Core.Database.Entities;

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
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
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
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type of the user, which must inherit from <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TRole">The type of the role, which must inherit from <see cref="Role{TKey}"/>.</typeparam>
/// <typeparam name="TResource">The type of the resource, which must inherit from <see cref="Resource{TKey}"/>.</typeparam>
public class UserManager<TKey, TUser, TRole, TResource> : IUserManager<TKey, TUser, TRole>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    protected readonly AclDbContext<TKey, TUser, TRole, TResource> Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserManager{TKey, TUser, TRole, TResource}"/> class.
    /// </summary>
    /// <param name="context">The ACL database context.</param>
    public UserManager(AclDbContext<TKey, TUser, TRole, TResource> context)
    {
        Context = context;
    }

    /// <inheritdoc />
    public virtual TUser UserProcessing(string userName, TRole roleForNewUsers)
    {
        var user = Context.Users.FirstOrDefault(x => x.Name == userName)
            ?? new TUser { Name = userName, RoleId = roleForNewUsers.Id };

        if (!user.Id.Equals(default)) return user;
        Context.Users.Add(user);
        Context.SaveChanges();

        return user;
    }

    /// <inheritdoc />
    public virtual async Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers)
    {
        var user = await Context.Users.FirstOrDefaultAsync(x => x.Name == userName)
            ?? new TUser { Name = userName, RoleId = roleForNewUsers.Id };

        if (!user.Id.Equals(default)) return user;
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        return user;
    }
}
