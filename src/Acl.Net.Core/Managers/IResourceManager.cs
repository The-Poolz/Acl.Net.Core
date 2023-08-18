using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Managers;

public interface IResourceManager : IResourceManager<int>
{
}

public interface IResourceManager<TKey> : IResourceManager<TKey, User<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
}

public interface IResourceManager<TKey, in TUser, TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TResource : Resource<TKey>
{
    public bool IsPermitted(TUser user, TResource resource);

    public Task<bool> IsPermittedAsync(TUser user, TResource resource);

    public TResource GetResourceByName(string resourceName);

    public Task<TResource> GetResourceByNameAsync(string resourceName);
}