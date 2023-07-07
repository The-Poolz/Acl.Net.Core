using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core;

public class AclManager : AclManager<int>
{
    public AclManager(AclDbContext context)
        : base(context, new RoleDataSeeder())
    { }
}

public class AclManager<TKey> : AclManager<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
    public AclManager(AclDbContext<TKey, User<TKey>, Role<TKey>, Resource<TKey>> context, IInitialDataSeeder<TKey, Role<TKey>> initialDataSeeder)
        : base(context, initialDataSeeder)
    { }
}

public class AclManager<TKey, TUser, TRole, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>, new()
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly AclDbContext<TKey, TUser, TRole, TResource> context;
    private readonly IInitialDataSeeder<TKey, TRole> initialDataSeeder;

    public AclManager(AclDbContext<TKey, TUser, TRole, TResource> context, IInitialDataSeeder<TKey, TRole> initialDataSeeder)
    {
        this.context = context;
        this.initialDataSeeder = initialDataSeeder;
    }

    public virtual bool IsPermitted(string userName, string resourceName)
    {
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
        if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentNullException(nameof(resourceName));

        var user = UserProcessing(userName, initialDataSeeder.SeedUserRole());

        var resource = context.Resources.FirstOrDefault(r => r.Name == resourceName)
            ?? throw new InvalidOperationException($"Resource with name '{resourceName}' not found.");

        return IsPermitted(user, resource);
    }

    public virtual bool IsPermitted(TUser user, TResource resource)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (resource == null) throw new ArgumentNullException(nameof(resource));

        return user.RoleId.Equals(initialDataSeeder.SeedAdminRole().Id) ||
            context.Resources.Any(r => r.RoleId.Equals(user.RoleId) && r.Id.Equals(resource.Id));
    }

    public virtual TUser UserProcessing(string userName, TRole roleForNewUsers)
    {
        var user = context.Users.FirstOrDefault(x => x.Name == userName);
        if (user != null) return user;

        var newUser = new TUser
        {
            Name = userName,
            RoleId = roleForNewUsers.Id
        };

        context.Users.Add(newUser);
        context.SaveChanges();

        return newUser;
    }
}
