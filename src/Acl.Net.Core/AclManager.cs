using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core;

public class AclManager<TUser, TKey> : AclManager<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>>
    where TUser : User<TKey>
    where TKey : IEquatable<TKey>
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

        var user = context.Users.FirstOrDefault(u => u.Id.Equals(claim.UserId));
        var resource = context.Resources.FirstOrDefault(r => r.Name == resourceName);
        if (user == null || resource == null) return false;

        return IsPermitted(user, resource);
    }

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        var userRoles  = context.Roles
            .Where(r => r.Id.Equals(resource.Id))
            .ToList();
        if (userRoles.Count == 0) return false;

        foreach (var role in userRoles)
        {
            var roleHasResource = context.Resources.Any(r => r.RoleId.Equals(role.Id) && r.Id.Equals(resource.Id));

            if (roleHasResource)
                return true;
        }

        return false;
    }

    public virtual void TokenProcessing(TUser user, TResource resource) =>
        throw new NotImplementedException();
}
