using Acl.Net.Core.Entities;

namespace Acl.Net.Core.Interfaces;

public interface IAcl<out TUser, TRole, in TResource>
    where TUser : User, new()
    where TRole : Role, new()
    where TResource : Resource, new()
{
    TRole[] Allow(TRole[] roles);
    TRole Allow(TRole role);
    TRole Allow(string roleName, TResource[] resources);
    void RemoveAllow(string roleName);
    TRole RemoveAllow(string roleName, string[] resources);
    TRole RemoveAllow(string roleName, string[] resources, string[] permissions);
    void RemoveResource(string resourceName);
    void RemovePermission(string permissionName);
    TRole AddRoleParent(string roleName, string parentName);
    TRole AddRoleParents(string roleName, string[] parents);
    TRole RemoveRoleParent(string roleName, string parentName);
    TRole RemoveRoleParents(string roleName, string[] parents);
    void AddUserRole(int userId, string roleName);
    void AddUserRole(string userId, string roleName);
    void AddUserRole(int userId, TRole role);
    void AddUserRole(string userId, TRole role);
    void AddUserRoles(int userId, string[] roleNames);
    void AddUserRoles(string userId, string[] roleNames);
    void AddUserRoles(int userId, TRole[] roles);
    void AddUserRoles(string userId, TRole[] roles);
    void RemoveUserRole(string userId, string roleName);
    void RemoveUserRole(int userId, string roleName);
    TRole[] UserRoles(string userId);
    TRole[] UserRoles(int userId);
    TUser[] RoleUsers(string roleName);
    TUser[] RoleUsers(TRole role);
    bool UserHasRole(string userId, string roleName);
    bool UserHasRole(string userId, TRole role);
    bool UserHasRole(int userId, string roleName);
    bool UserHasRole(int userId, TRole role);
    string[] AllowedPermissions(string userId, string resource);
    string[] AllowedPermissions(int userId, string resource);
    bool IsAllowed(string userId, string resource, string permission);
    bool IsAllowed(int userId, string resource, string permission);
    bool IsAllowed(string userId, TResource resource);
    bool IsAllowed(int userId, TResource resource);
    bool IsRoleAllowed(string roleName, string resource, string permission);
    bool IsRoleAllowed(string roleName, TResource resource);
}