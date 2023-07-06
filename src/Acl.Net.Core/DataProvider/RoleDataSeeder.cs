using Acl.Net.Core.Entities;

namespace Acl.Net.Core.DataProvider;

public class RoleDataSeeder : IInitialDataSeeder<int, Role<int>>
{
    public Role<int> SeedAdminRole()
    {
        return new Role<int> { Id = 1, Name = "AdminRole" };
    }

    public Role<int> SeedUserRole()
    {
        return new Role<int> { Id = 2, Name = "UserRole" };
    }
}