using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Services;

public class UserService<TUser, TRole, TResource, TClaim> : IUserService<TUser, TRole>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim
{
    private readonly AclDbContext<TUser, TRole, TResource, TClaim> context;

    public UserService(AclDbContext<TUser, TRole, TResource, TClaim> context)
    {
        this.context = context;
    }

    public virtual TUser? GetUser(Guid id) =>
        context.Users.FirstOrDefault(x => x.Id == id);

    public virtual TUser? GetUser(string userToken)
    {
        var claim = context.Claims.FirstOrDefault(x => x.Token == userToken);
        return claim != null ? context.Users.FirstOrDefault(u => u.Id == claim.UserId) : null;
    }

    public virtual ICollection<TRole>? GetUserRoles(Guid userId)
    {
        var roles = context.Roles
            .Where(r => r.UserId == userId)
            .ToList();

        return roles.Count > 0 ? roles : null;
    }
}