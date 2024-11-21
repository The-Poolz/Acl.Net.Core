using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Database;

/// <summary>
/// Provides methods for seeding the initial roles within the system.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the role.</typeparam>
/// <typeparam name="TRole">The type of the role being seeded.</typeparam>
public interface IInitialDataSeeder<TKey, out TRole>
    where TKey : IEquatable<TKey>
    where TRole : Role<TKey>
{
    /// <summary>
    /// Seeds the administrative role within the system.
    /// </summary>
    /// <returns>A <see cref="TRole"/> representing the administrative role.</returns>
    public TRole SeedAdminRole();
}
