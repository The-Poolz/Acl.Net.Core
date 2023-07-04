using Acl.Net.Core.Entities;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Services;

public class UserService<TUser, TRole, TResource, TClaim> : IUserService<TUser>
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim
{
    private readonly AclDbContext<TUser, TRole, TResource, TClaim> _context;

    public UserService(AclDbContext<TUser, TRole, TResource, TClaim> context)
    {
        _context = context;
    }

    public virtual TUser? GetUser(Guid id) =>
        _context.Users.FirstOrDefault(x => x.Id == id);
}