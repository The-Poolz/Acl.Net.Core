using Acl.Net.Core.Secrets;
using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;
using Acl.Net.Core.Cryptography;

namespace Acl.Net.Core;

public class AclManager : AclManager<int, User>
{
    public AclManager(AclDbContext context, ISecretsProvider secretsProvider)
        : base(context, secretsProvider)
    { }
}

public class AclManager<TKey, TUser> : AclManager<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
{
    public AclManager(AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>> context, ISecretsProvider secretsProvider)
        : base(context, secretsProvider)
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
    private readonly UserTokenManager userTokenManager;

    public AclManager(AclDbContext<TKey, TUser, TRole, TResource, TClaim> context, ISecretsProvider secretsProvider)
    {
        this.context = context;
        userTokenManager = new UserTokenManager(secretsProvider);
    }

    public virtual bool IsPermitted(string userName, string resourceName, string? roleNameForNewUsers = null)
    {
        var user = UserProcessing(userName, roleNameForNewUsers);

        var resource = context.Resources.FirstOrDefault(r => r.Name == resourceName)
            ?? throw new InvalidOperationException($"Resource with name '{resourceName}' not found.");

        _ = ClaimProcessing(user, resource);

        return IsPermitted(user, resource);
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

        if (!string.IsNullOrEmpty(roleNameForNewUsers))
        {
            var roleForNewUser = context.Roles.FirstOrDefault(x => x.Name == roleNameForNewUsers)
                ?? throw new InvalidOperationException($"Role with name '{roleNameForNewUsers}' not found.");
            newUser.RoleId = roleForNewUser.Id;
        }

        context.Users.Add(newUser);
        context.SaveChanges();

        return newUser;
    }

    public virtual TClaim ClaimProcessing(TUser user, TResource resource)
    {
        var existingClaim = context.Claims
            .OrderByDescending(x => x.DateOfCreation)
            .FirstOrDefault(x => x.UserId.Equals(user.Id));

        if (existingClaim != null && DateTime.UtcNow <= existingClaim.DateOfCreation.AddDays(1))
            return existingClaim;

        var newToken = userTokenManager.GenerateToken(user.Id);

        var newClaim = new TClaim
        {
            Token = newToken,
            DateOfCreation = DateTime.UtcNow,
            UserId = user.Id
        };

        context.Claims.Add(newClaim);
        context.SaveChanges();

        return newClaim;
    }
}
