using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Services;

public interface IClaimService<in TUser, out TClaim>
    where TUser : User
    where TClaim : Claim
{
    public TClaim? GetClaim(TUser user);

    public TClaim AddClaim(TUser user);

    public TClaim UpdateClaim(TUser user);
}