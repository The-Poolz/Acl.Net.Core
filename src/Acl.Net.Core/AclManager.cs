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
    where TUser : User<TKey>
{
    public AclManager(AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>> context)
        : base(context)
    { }
}

public class AclManager<TKey, TUser, TRole, TResource, TClaim>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
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
        var userRoles  = context.Roles
            .Where(r => r.Id.Equals(resource.Id) && r.UserId.Equals(user.Id))
            .ToList();

        return userRoles.Count != 0 && userRoles
            .Select(role => context.Resources.Any(r => r.RoleId.Equals(role.Id) && r.Id.Equals(resource.Id)))
            .Any(roleHasResource => roleHasResource);
    }

    public virtual void TokenProcessing(TUser user, TResource resource) =>
        throw new NotImplementedException();
}
