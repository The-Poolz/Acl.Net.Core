using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Managers;

public interface IAclManager : IAclManager<int>
{
}

public interface IAclManager<TKey> : IAclManager<TKey, User<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
}

public interface IAclManager<TKey, in TUser, in TResource>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TResource : Resource<TKey>
{
    public bool IsPermitted(string userName, string resourceName);

    public bool IsPermitted(TUser user, string resourceName);

    public bool IsPermitted(string userName, TResource resource);

    public bool IsPermitted(TUser user, TResource resource);
    
    public IEnumerable<TResource> IsPermitted(string userName, IEnumerable<string> resourceNames);

    public Task<bool> IsPermittedAsync(string userName, string resourceName);

    public Task<bool> IsPermittedAsync(TUser user, string resourceName);

    public Task<bool> IsPermittedAsync(string userName, TResource resource);

    public Task<bool> IsPermittedAsync(TUser user, TResource resource);
    
    public Task<IEnumerable<TResource>> IsPermittedAsync(string userName, IEnumerable<string> resourceNames);
}