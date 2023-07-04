using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Services;

public interface IUserService<out TUser, TRole>
    where TUser : User
    where TRole : Role
{
    public TUser? GetUser(Guid id);
    public TUser? GetUser(string userToken);
    public ICollection<TRole>? GetUserRoles(Guid userId);
}
