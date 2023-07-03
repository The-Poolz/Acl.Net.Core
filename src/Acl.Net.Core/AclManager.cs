using Acl.Net.Core.Entities;

namespace Acl.Net.Core;

public class AclManager<TUser, TRole, TResource, TClaim>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim
{
    public virtual bool IsPermitted(string userToken, string resourceName) =>
        throw new NotImplementedException();

    public virtual bool IsPermitted(TUser user, string resourceName) =>
        throw new NotImplementedException();

    public virtual bool IsPermitted(TUser user, TResource resource) =>
        throw new NotImplementedException();

    public virtual void TokenProcessing(TUser user, TResource resource) =>
        throw new NotImplementedException();
}