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

        context.Claims.AddRange(Claims);
        context.Users.AddRange(Users);
        context.Roles.AddRange(Roles);
        context.Resources.AddRange(Resources);

        context.SaveChanges();

        return context;
    }

    internal static List<Claim> Claims
    {
        get
        {
            Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");

            return new List<Claim>
            {
                new()
                {
                    Id = 1,
                    Token = "z2UM1bk+CEmkBFTupu40wPopntPcdA3GsD8kNeFk0qOIZu9DQ6/w/GUd4+STM+nPxvqfqBGCt2nH+hu56RlitQ==",
                    DateOfCreation = DateTime.UtcNow.AddDays(-1),
                    UserId = 1
                },
                new()
                {
                    Id = 2,
                    Token = "e6ZrelrEpQSYzZoJ4gZEcpa2LMdnTLeD7dv2tcsGk1tbNlC4aU4AKy4mCwlXwy760syEy/ifi0KuI7mTCT/GDA==",
                    DateOfCreation = DateTime.UtcNow,
                    UserId = 1
                },
                new()
                {
                    Id = 3,
                    Token = "A4fPmeCsxOcfJZxsHDuabX2Js8UQNtz2K+B6Fv3Ze6EQYXeQR5nN4Zv4jIlyZHrUB8sAstZTYjeOce4/3Jas2g==",
                    DateOfCreation = DateTime.UtcNow.AddDays(-1),
                    UserId = 2
                },
                new()
                {
                    Id = 4,
                    Token = "le66WawxfMKdAQ5t5M++TxHMe3yBY+GIns7lNDmbY1FoKjLyOS29hDw73VIH4MHw+/YcRONs1KFtDylYG1ptuA==",
                    DateOfCreation = DateTime.UtcNow,
                    UserId = 2
                }
            };
        }
    }

    internal static List<User> Users => new()
    {
        new User { Id = 1 },
        new User { Id = 2 }
    };

    internal static List<Role> Roles => new()
    {
        new Role { Id = 1, UserId = 1, Name = "User" },
        new Role { Id = 2, UserId = 2, Name = "Admin" }
    };

    internal static List<Resource> Resources => new()
    {
        new Resource { Id = 1, Name = "publicResource", RoleId = 1 },
        new Resource { Id = 2, Name = "privateResource", RoleId = 2 }
    };
}