using Acl.Net.Core.Entities;
using Acl.Net.Core.Services;
using Acl.Net.Core.Cryptography;

namespace Acl.Net.Core.MicrosoftSqlServer.Services;

public class ClaimService<TUser, TRole, TResource, TClaim> : IClaimService<TUser, TClaim>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim, new()
{
    private readonly AclDbContext<TUser, TRole, TResource, TClaim> _context;

    public ClaimService(AclDbContext<TUser, TRole, TResource, TClaim> context)
    {
        _context = context;
    }

    public virtual TClaim? GetClaim(TUser user) =>
        _context.Claims
            .OrderByDescending(x => x.DateOfCreation)
            .FirstOrDefault(x => x.UserId == user.Id);

    public virtual TClaim AddClaim(TUser user)
    {
        var newToken = UserTokenManager.GenerateToken(user.Id);

        var newClaim = new TClaim
        {
            Token = newToken,
            DateOfCreation = DateTime.UtcNow,
            UserId = user.Id
        };

        _context.Claims.Add(newClaim);
        _context.SaveChanges();

        return newClaim;
    }

    public virtual TClaim UpdateClaim(TUser user)
    {
        var existingClaim = GetClaim(user);

        if (existingClaim != null && DateTime.UtcNow <= existingClaim.DateOfCreation.AddDays(1))
            return existingClaim;

        return AddClaim(user);
    }
}