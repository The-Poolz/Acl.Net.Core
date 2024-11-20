using Acl.Net.Core.Database;
using Microsoft.EntityFrameworkCore;
using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Managers.Tests.Mock;

internal static class InMemoryAclDbContext
{
    private static readonly RoleDataSeeder RoleDataSeeder = new();
    private static readonly Role<int> UserRole = RoleDataSeeder.SeedUserRole();
    private static readonly Role<int> AdminRole = RoleDataSeeder.SeedAdminRole();

    internal static User UserAccount = new()
    {
        Id = 1, Name = "UserAccount", RoleId = UserRole.Id
    };

    internal static User AdminAccount = new()
    {
        Id = 2,
        Name = "AdminAccount",
        RoleId = AdminRole.Id
    };

    internal static Resource PublicResource = new()
    {
        Id = 1,
        Name = "PublicResource",
        RoleId = UserRole.Id
    };

    internal static Resource PrivateResource = new()
    {
        Id = 2,
        Name = "PrivateResource",
        RoleId = AdminRole.Id
    };

    internal static List<User> Users => [UserAccount, AdminAccount];

    internal static List<Resource> Resources => [PublicResource, PrivateResource];

    internal static AclDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AclDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AclDbContext(options);

        context.Users.AddRange(Users);
        context.Resources.AddRange(Resources);

        context.SaveChanges();

        return context;
    }
}