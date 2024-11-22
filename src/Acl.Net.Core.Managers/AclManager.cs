using Acl.Net.Core.Database;
using Acl.Net.Core.Database.Entities;
using Acl.Net.Core.Managers.Exceptions;

namespace Acl.Net.Core.Managers;

/// <summary>
/// Manages access control lists (ACLs) using integer keys.<br/>
/// This class provides a simplified interface for managing ACLs with integer keys, by extending the more generic <see cref="AclManager{TKey}"/>.
/// </summary>
public class AclManager : AclManager<int>, IAclManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager"/> class and with the default <see cref="RoleDataSeeder"/>.
    /// </summary>
    /// <param name="context">An implementation, or default <see cref="AclDbContext"/>.</param>
    public AclManager(
        AclDbContext context
    )
        : this(new RoleDataSeeder(), context)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager{TKey}"/> class
    /// with the provided <see cref="IInitialDataSeeder{TKey,TRole}"/> and <see cref="AclDbContext{TKey}"/>.
    /// </summary>
    /// <param name="initialDataSeeder">An implementation of <see cref="IInitialDataSeeder{TKey,TRole}"/> used to seed initial role data.</param>
    /// <param name="context">An implementation, or default <see cref="AclDbContext"/>.</param>
    public AclManager(
        IInitialDataSeeder<int, Role<int>> initialDataSeeder,
        AclDbContext context
    )
        : base(initialDataSeeder, context)
    { }
}

/// <summary>
/// Manages access control lists (ACLs) using keys of type <see cref="TKey"/>.<br/>
/// This class provides the base functionality for managing ACLs with specified key types.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
public class AclManager<TKey> : AclManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>, IAclManager<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager{TKey}"/> class
    /// with the provided <see cref="IInitialDataSeeder{TKey,TRole}"/> and <see cref="AclDbContext{TKey}"/>.
    /// </summary>
    /// <param name="initialDataSeeder">An implementation of <see cref="IInitialDataSeeder{TKey,TRole}"/> used to seed initial role data.</param>
    /// <param name="context">An implementation, or default <see cref="AclDbContext{TKey}"/>.</param>
    public AclManager(
        IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder,
        AclDbContext<TKey> context
    )
        : base(initialDataSeeder, context)
    { }
}

/// <summary>
/// Manages access control lists (ACLs) using keys, users, roles, and resources of specified types. <br/>
/// This class provides the complete functionality for managing ACLs with specified key, user, role, and resource types.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
/// <typeparam name="TUser">The type of the user, which must be a derived type of <see cref="User{TKey}"/>.</typeparam>
/// <typeparam name="TRole">The type of the role, which must be a derived type of <see cref="Role{TKey}"/></typeparam>
/// <typeparam name="TResource">The type of the resource, which must be a derived type of <see cref="Resource{TKey}"/></typeparam>
public class AclManager<TKey, TUser, TRole, TResource> : IAclManager<TKey, TUser, TRole, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    protected readonly IInitialDataSeeder<TKey, TRole> InitialDataSeeder;
    protected readonly AclDbContext<TKey, TUser, TRole, TResource> Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AclManager{TKey,TUser,TRole,TResource}"/> class
    /// with the provided <see cref="IInitialDataSeeder{TKey,TRole}"/> and <see cref="AclDbContext{TKey,TUser,TRole,TResource}"/>.
    /// </summary>
    /// <param name="initialDataSeeder">An implementation of <see cref="IInitialDataSeeder{TKey, TRole}"/> used to seed initial role data.</param>
    /// <param name="context">An implementation, or default <see cref="AclDbContext{TKey,TUser,TRole,TResource}"/>.</param>
    public AclManager(
        IInitialDataSeeder<TKey, TRole> initialDataSeeder,
        AclDbContext<TKey, TUser, TRole, TResource> context
    )
    {
        InitialDataSeeder = initialDataSeeder;
        Context = context;
    }

    /// <inheritdoc />
    public bool IsPermitted(string userName, string resourceName)
    {
        var user = GetUserByName(userName);
        return IsAdmin(user) || GetUserRoles(user.Name).Any(role => IsPermitted(role, GetResourceByName(resourceName)));
    }

    /// <inheritdoc />
    public bool IsPermitted(TUser user, TResource resource)
    {
        return IsAdmin(user) || GetUserRoles(user.Name).Any(role => IsPermitted(role, resource));
    }

    /// <inheritdoc />
    public bool IsPermitted(TRole role, TResource resource)
    {
        return IsAdmin(role) || Context.Resources.Any(res => res.Name.Equals(resource.Name) && res.RoleId.Equals(role.Id));
    }

    public bool IsAdmin(TKey roleId) => roleId.Equals(InitialDataSeeder.SeedAdminRole().Id);
    public bool IsAdmin(TUser user) => IsAdmin(user.RoleId);
    public bool IsAdmin(TRole role) => IsAdmin(role.Id);

    public IEnumerable<TRole> GetUserRoles(string userName)
    {
        return Context.Roles
            .Where(r => Context.Users
                .Where(u => u.Name == userName)
                .Select(u => u.RoleId)
                .Contains(r.Id))
            .ToArray();
    }

    private TResource GetResourceByName(string resourceName)
    {
        return Context.Resources.FirstOrDefault(r => r.Name == resourceName) ?? throw new ResourceNotFoundException(resourceName);
    }

    private TUser GetUserByName(string userName)
    {
        return Context.Users.FirstOrDefault(r => r.Name == userName) ?? throw new UserNotFoundException(userName);
    }
}
