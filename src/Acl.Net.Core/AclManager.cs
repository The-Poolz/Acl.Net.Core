using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core;

public class AclManager : AclManager<int, User>
{
    public AclManager(AclDbContext context)
        : base(context)
    { }
}

public class AclManager<TKey, TUser> : AclManager<TKey, TUser, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
{
    public AclManager(AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>> context)
        : base(context)
    { }
}

public class AclManager<TKey, TUser, TRole, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource> context;

    public AclManager(AclDbContext<TKey, TUser, TRole, TResource> context)
    {
        this.context = context;
    }

    public virtual bool IsPermitted(string userName, string resourceName, string? roleNameForNewUsers = null)
    {
        var user = UserProcessing(userName, roleNameForNewUsers);

        var resource = context.Resources.FirstOrDefault(r => r.Name == resourceName)
            ?? throw new InvalidOperationException($"Resource with name '{resourceName}' not found.");

        return IsPermitted(user, resource);
    }

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return context.Resources.Any(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    public virtual TUser UserProcessing(string userName, string? roleNameForNewUsers = null)
    {
        var user = context.Users.FirstOrDefault(x => x.Name == userName);
        if (user != null) return user;

        var newUser = new TUser
        {
            Name = userName
        };

        if (!string.IsNullOrEmpty(roleNameForNewUsers))
        {
            var roleForNewUser = context.Roles.FirstOrDefault(x => x.Name == roleNameForNewUsers)
                ?? throw new InvalidOperationException($"Role with name '{roleNameForNewUsers}' not found.");
            newUser.RoleId = roleForNewUser.Id;
        }

        context.Users.Add(newUser);
        context.SaveChanges();

        return newUser;
    }
}
