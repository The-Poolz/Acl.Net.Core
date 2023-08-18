using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Managers;

public interface IUserManager : IUserManager<int>
{
}

public interface IUserManager<TKey> : IUserManager<TKey, User<TKey>, Role<TKey>>
    where TKey : IEquatable<TKey>
{
}

public interface IUserManager<TKey, TUser, in TRole>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
{
    public TUser UserProcessing(string userName, TRole roleForNewUsers);

    public Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers);
}