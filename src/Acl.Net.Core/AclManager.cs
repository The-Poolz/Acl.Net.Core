using Acl.Net.Core.Entities;
using Acl.Net.Core.Services;
using Acl.Net.Core.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core;

public class AclManager<TUser> : AclManager<TUser, Role, Resource, Claim>
    where TUser : User
{
    public AclManager(AclDbContext<TUser, Role, Resource, Claim> context)
        : base(context)
    { }
}

public class AclManager<TUser, TRole, TResource, TClaim>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim, new()
{
    private readonly AclDbContext<TUser, TRole, TResource, TClaim> context;
    private readonly IUserService<TUser, TRole> userService;
    private readonly IClaimService<TUser, TClaim> claimService;

    public AclManager(AclDbContext<TUser, TRole, TResource, TClaim> context)
    {
        this.context = context;
        userService = new UserService<TUser,TRole,TResource,TClaim>(context);
        claimService = new ClaimService<TUser, TRole, TResource, TClaim>(context);
    }

    public virtual bool IsPermitted(string userToken, string resourceName)
    {
        var user = userService.GetUser(userToken);
        var resource = context.Resources.FirstOrDefault(r => r.Name == resourceName);

        if (user == null || resource == null)
            return false;

        return IsPermitted(user, resource);
    }

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        var userRoles = userService.GetUserRoles(user.Id);

        if (userRoles == null) return false;

        foreach (var role in userRoles)
        {
            var roleHasResource = context.Resources.Any(r => r.RoleId == role.Id && r.Id == resource.Id);

            if (roleHasResource)
                return true;
        }

        return false;
    }

    public virtual void TokenProcessing(TUser user, TResource resource) =>
        throw new NotImplementedException();
}
