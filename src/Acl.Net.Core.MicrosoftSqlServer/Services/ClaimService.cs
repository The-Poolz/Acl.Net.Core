using Acl.Net.Core.Entities;
using Acl.Net.Core.Services;

namespace Acl.Net.Core.MicrosoftSqlServer.Services;

public class ClaimService<TUser, TRole, TResource, TClaim> : IClaimService<TClaim>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim
{
    private readonly AclDbContext<TUser, TRole, TResource, TClaim> _context;

    public ClaimService(AclDbContext<TUser, TRole, TResource, TClaim> context)
    {
        _context = context;
    }

    public virtual string? GetToken(TUser user) =>
        _context.Claims.FirstOrDefault(x => x.UserId == user.Id)?.Token;
}