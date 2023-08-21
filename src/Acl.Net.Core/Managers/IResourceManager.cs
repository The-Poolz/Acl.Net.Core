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
    public bool IsPermitted(TUser user, string resourceName);

    public bool IsPermitted(TUser user, TResource resource);

    /// <summary>
    /// Check if the user is permitted for at least one resource in resources.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Return <see langword="true" /> if at least one resource is allowed to the user, otherwise <see langword="false" />.</returns>
    public bool IsPermitted(TUser user, IEnumerable<TResource> resources);
    
    public Task<bool> IsPermittedAsync(TUser user, string resourceName);

    public Task<bool> IsPermittedAsync(TUser user, TResource resource);

    /// <summary>
    /// Check if the user is permitted for at least one resource in resources.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Return <see langword="true" /> if at least one resource is allowed to the user, otherwise <see langword="false" />.</returns>
    public Task<bool> IsPermittedAsync(TUser user, IEnumerable<TResource> resources);

    public TResource GetResourceByName(string resourceName);

    public Task<TResource> GetResourceByNameAsync(string resourceName);

    public IEnumerable<TResource> GetResourcesByName(string resourceName);
}