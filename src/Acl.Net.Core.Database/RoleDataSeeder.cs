using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Database;

/// <inheritdoc cref="IInitialDataSeeder{TKey,TRole}"  />
public class RoleDataSeeder : IInitialDataSeeder<int, Role<int>>
{
    /// <inheritdoc cref="IInitialDataSeeder{TKey,TRole}.SeedAdminRole()" />
    public Role<int> SeedAdminRole()
    {
        return new Role<int> { Id = 1, Name = "AdminRole" };
    }
}
