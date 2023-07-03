using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Services;

public interface IUserService<out TUser>
    where TUser : User
{
    public TUser? GetUser(Guid id);
}
