using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Interfaces;

public interface IBackend<TUser, TRole>
    where TUser : User
    where TRole : Role
{
    TRole CreateRole(TRole role);
    TRole UpdateRole(TRole role);
    void DeleteRole(TRole role);
    TRole? GetRole(string roleName);
    void DeleteResource(string resourceName);
    void DeletePermission(string permissionName);
    TRole AddRoleParents(TRole role, string[] parents);
    TRole RemoveRoleParents(TRole role, string[] parents);
    TUser? GetUser(string userId);
    TUser[] GetUsers(string roleName);
    TUser CreateUser(TUser user);
    TUser UpdateUser(TUser user);
}
