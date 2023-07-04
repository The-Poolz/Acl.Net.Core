using Acl.Net.Core.Entities;
using Acl.Net.Core.Cryptography;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Services;

public class ClaimService<TKey, TUser, TRole, TResource, TClaim>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
    where TClaim : Claim<TKey>, new()
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource, TClaim> _context;

    public ClaimService(AclDbContext<TKey, TUser, TRole, TResource, TClaim> context)
    {
        _context = context;
    }

    public virtual TClaim? GetClaim(TUser user) =>
        _context.Claims
            .OrderByDescending(x => x.DateOfCreation)
            .FirstOrDefault(x => x.UserId.Equals(user.Id));

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