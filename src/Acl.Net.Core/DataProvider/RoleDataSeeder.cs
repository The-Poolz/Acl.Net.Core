using Acl.Net.Core.Entities;

namespace Acl.Net.Core.DataProvider;

public class RoleDataSeeder : IInitialDataSeeder<Role<int>, int>
{
    public Role<int>[] SeedRoles()
    {
        return new[]
        {
            new Role<int> { Id = 1, Name = "AdminRole" },
            new Role<int> { Id = 2, Name = "UserRole" }
        };
    }
}