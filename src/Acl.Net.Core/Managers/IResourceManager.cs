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
    /// Check which resources are allowed for user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resourceNames">Resources that will be checked for the user.</param>
    /// <returns>Returns the allowed resources for the user.</returns>
    public IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<string> resourceNames);

    /// <summary>
    /// Check which resources are allowed for user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Returns the allowed resources for the user.</returns>
    public IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<TResource> resources);

    public Task<bool> IsPermittedAsync(TUser user, string resourceName);

    public Task<bool> IsPermittedAsync(TUser user, TResource resource);

    /// <summary>
    /// Check which resources are allowed for user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resourceNames">Resources that will be checked for the user.</param>
    /// <returns>Returns the allowed resources for the user.</returns>
    public Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<string> resourceNames);

    /// <summary>
    /// Check which resources are allowed for user.
    /// </summary>
    /// <param name="user">User for whom resources are being checked.</param>
    /// <param name="resources">Resources that will be checked for the user.</param>
    /// <returns>Returns the allowed resources for the user.</returns>
    public Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<TResource> resources);

    public TResource GetResourceByName(string resourceName);

    public Task<TResource> GetResourceByNameAsync(string resourceName);
}