using Acl.Net.Core.Entities;

namespace Acl.Net.Core.DataProvider;

/// <summary>
/// Provides implementations for seeding the initial administrative and user roles within the system.
/// </summary>
public class RoleDataSeeder : IInitialDataSeeder<int, Role<int>>
{
    /// <summary>
    /// Seeds the administrative role with predefined properties.
    /// </summary>
    /// <returns>A <see cref="Role{TKey}"/> representing the administrative role.</returns>
    public Role<int> SeedAdminRole()
    {
        return new Role<int> { Id = 1, Name = "AdminRole" };
    }

    /// <summary>
    /// Seeds the user role with predefined properties.
    /// </summary>
    /// <returns>A <see cref="Role{TKey}"/> representing the user role.</returns>
    public Role<int> SeedUserRole()
    {
        return new Role<int> { Id = 2, Name = "UserRole" };
    }
}
