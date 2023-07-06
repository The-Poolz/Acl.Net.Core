using Acl.Net.Core.Entities;

namespace Acl.Net.Core.DataProvider;

public interface IInitialDataSeeder<out TRole, TKey>
    where TRole : Role<TKey>
    where TKey : IEquatable<TKey>
{
    TRole SeedAdminRole();
    TRole SeedUserRole();
}