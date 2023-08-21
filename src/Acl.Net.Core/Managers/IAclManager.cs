using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Managers;

public interface IAclManager : IAclManager<int>
{
}

public interface IAclManager<TKey> : IAclManager<TKey, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
}

public interface IAclManager<TKey, TResource>
    where TKey : IEquatable<TKey>
    where TResource : Resource<TKey>
{
    public bool IsPermitted(string userName, string resourceName);

    public IEnumerable<TResource> IsPermitted(string userName, IEnumerable<string> resourceNames);

    public Task<bool> IsPermittedAsync(string userName, string resourceName);

    public Task<IEnumerable<TResource>> IsPermittedAsync(string userName, IEnumerable<string> resourceNames);
}