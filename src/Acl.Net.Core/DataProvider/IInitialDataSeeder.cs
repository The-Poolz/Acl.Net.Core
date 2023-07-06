using Acl.Net.Core.Entities;

namespace Acl.Net.Core.DataProvider;

public interface IInitialDataSeeder<TKey, out TRole>
    where TKey : IEquatable<TKey>
    where TRole : Role<TKey>
{
    TRole SeedAdminRole();
    TRole SeedUserRole();
}
