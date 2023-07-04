using Acl.Net.Core.Entities;
using Acl.Net.Core.Services;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core;

public class AclManager<TUser, TRole, TResource, TClaim>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim, new()
{
    private readonly AclDbContext<TUser, TRole, TResource, TClaim> context;
    private readonly IUserService<TUser> userService;
    private readonly IClaimService<TUser, TClaim> claimService;

    public AclManager(AclDbContext<TUser, TRole, TResource, TClaim> context)
    {
        this.context = context;
        userService = new UserService<TUser,TRole,TResource,TClaim>(context);
        claimService = new ClaimService<TUser, TRole, TResource, TClaim>(context);
    }

    public virtual bool IsPermitted(string userToken, string resourceName) =>
        throw new NotImplementedException();

    public virtual bool IsPermitted(TUser user, string resourceName) =>
        throw new NotImplementedException();

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        return context.Users
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Roles)
            .Any(r => r.Resources.Any(res => res.Id == resource.Id));
    }

    public virtual void TokenProcessing(TUser user, TResource resource) =>
        throw new NotImplementedException();
}
