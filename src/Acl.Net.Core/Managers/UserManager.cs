using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.Managers;

public class UserManager : UserManager<int>, IUserManager
{
    public UserManager(AclDbContext context)
        : base(context)
    { }
}

public class UserManager<TKey> : UserManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>, IUserManager<TKey>
    where TKey : IEquatable<TKey>
{
    public UserManager(AclDbContext<TKey> context)
        : base(context)
    { }
}

public class UserManager<TKey, TUser, TRole, TResource> : IUserManager<TKey, TUser, TRole>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource> context;

    public UserManager(AclDbContext<TKey, TUser, TRole, TResource> context)
    {
        this.context = context;
    }

    public TUser UserProcessing(string userName, TRole roleForNewUsers)
    {
        var user = context.Users.FirstOrDefault(x => x.Name == userName)
            ?? new TUser { Name = userName, RoleId = roleForNewUsers.Id };

        if (!user.Id.Equals(default)) return user;
        context.Users.Add(user);
        context.SaveChanges();

        return user;
    }

    public async Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Name == userName)
            ?? new TUser { Name = userName, RoleId = roleForNewUsers.Id };

        if (!user.Id.Equals(default)) return user;
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }
}
