using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.Tests.Mock;

internal static class InMemoryAclDbContext
{
    private static readonly RoleDataSeeder roleDataSeeder = new();
    private static readonly Role<int> userRole = roleDataSeeder.SeedUserRole();
    private static readonly Role<int> adminRole = roleDataSeeder.SeedAdminRole();

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

    internal static List<User> Users => new()
    {
        new User { Id = 1, Name = "UserAccount", RoleId = userRole.Id},
        new User { Id = 2, Name = "AdminAccount", RoleId = adminRole.Id}
    };

    internal static List<Resource> Resources => new()
    {
        new Resource { Id = 1, Name = "PublicResource", RoleId = userRole.Id },
        new Resource { Id = 2, Name = "PrivateResource", RoleId = adminRole.Id }
    };
}