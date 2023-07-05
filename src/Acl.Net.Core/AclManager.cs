using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core;

public class AclManager : AclManager<int, User>
{
    public AclManager(AclDbContext context)
        : base(context)
    { }
}

public class AclManager<TKey, TUser> : AclManager<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
{
    public AclManager(AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>> context)
        : base(context)
    { }
}

public class AclManager<TKey, TUser, TRole, TResource, TClaim>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
    where TClaim : Claim<TKey>, new()
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource, TClaim> context;

    public AclManager(AclDbContext<TKey, TUser, TRole, TResource, TClaim> context)
    {
        this.context = context;
    }

    public virtual bool IsPermitted(string userToken, string resourceName)
    {
        var claim = context.Claims.FirstOrDefault(c => c.Token == userToken);
        if (claim == null) return false;

        var user = context.Users.FirstOrDefault(u => u.Id.Equals(claim.UserId))!;
        var resource = context.Resources.FirstOrDefault(r => r.Name == resourceName);
        return resource != null && IsPermitted(user, resource);
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

        if (string.IsNullOrEmpty(roleNameForNewUsers))
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
