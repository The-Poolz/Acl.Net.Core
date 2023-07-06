using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.Tests.Mock;

internal static class InMemoryAclDbContext
{
    internal static AclDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AclDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AclDbContext(options);

        context.Users.AddRange(Users);
        context.Roles.AddRange(Roles);
        context.Resources.AddRange(Resources);

        context.SaveChanges();

        return context;
    }

    internal static List<User> Users => new()
    {
        new User { Id = 1, Name = "UserAccount", RoleId = 1},
        new User { Id = 2, Name = "AdminAccount", RoleId = 2}
    };

    internal static List<Role> Roles => new()
    {
        new Role { Id = 1, Name = "UserRole" },
        new Role { Id = 2, Name = "AdminRole" }
    };

    internal static List<Resource> Resources => new()
    {
        new Resource { Id = 1, Name = "PublicResource", RoleId = 1 },
        new Resource { Id = 2, Name = "PrivateResource", RoleId = 2 }
    };
}