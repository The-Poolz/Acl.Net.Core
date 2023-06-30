using Acl.Net.Core.Entities;
using Acl.Net.Core.Exceptions;
using Acl.Net.Core.Interfaces;

namespace Acl.Net.Core;

public class Acl<TUser, TRole, TResource> : IAcl<TUser, TRole, TResource>
    where TUser : User, new()
    where TRole : Role, new()
    where TResource : Resource, new()
{
    private readonly IBackend<TUser, TRole> _backendService;

    public Acl(IBackend<TUser, TRole> backendService)
    {
        _backendService = backendService;
    }

    public TRole Allow(TRole role) =>
        _backendService.GetRole(role.Name) != null
            ? _backendService.UpdateRole(role)
            : _backendService.CreateRole(role);

    public TRole[] Allow(TRole[] roles) =>
        roles.Select(Allow).ToArray();

    public TRole Allow(string roleName, TResource[] resources) =>
        Allow(new TRole
        {
            Name = roleName,
            Resources = resources
        });

    public void RemoveAllow(string roleName) =>
        _backendService.DeleteRole(_backendService.GetRole(roleName) ?? throw new RoleNotFoundException());

    public TRole RemoveAllow(string roleName, string[] resources)
    {
        var role = _backendService.GetRole(roleName) ?? throw new RoleNotFoundException();
        if (resources.Length != 0)
            role.Resources = role.Resources.Where(r => !Array.Exists(resources, n => n == r.Name)).ToArray();
        _backendService.UpdateRole(role);
        return role;
    }

    public TRole RemoveAllow(string roleName, string[] resources, string[] permissions)
    {
        var role = RemoveAllow(roleName, resources);
        if (permissions.Length != 0)
            role.Resources = role.Resources.Where(r =>
            {
                r.Permissions = r.Permissions.Where(p => !Array.Exists(permissions, pe => pe == p.Name)).ToArray();
                return true;
            }).ToArray();
        _backendService.UpdateRole(role);
        return role;
    }

    public void RemoveResource(string resourceName) =>
        _backendService.DeleteResource(resourceName);

    public void RemovePermission(string permissionName) =>
        _backendService.DeletePermission(permissionName);

    public TRole AddRoleParents(string roleName, string[] parents)
    {
        var role = _backendService.GetRole(roleName) ?? throw new RoleNotFoundException();
        if (parents.Any(parent => _backendService.GetRole(parent) == null))
        {
            throw new RoleParentNotFoundException();
        }
        return _backendService.UpdateRole(_backendService.AddRoleParents(role, parents));
    }

    public TRole AddRoleParent(string roleName, string parentName) =>
        AddRoleParents(roleName, new[]
        {
            parentName
        });

    public TRole RemoveRoleParents(string roleName, string[] parents) =>
        _backendService.UpdateRole(_backendService.RemoveRoleParents(_backendService.GetRole(roleName) ?? throw new RoleNotFoundException(), parents));

    public TRole RemoveRoleParent(string roleName, string parentName) =>
        RemoveRoleParents(roleName, new[]
        {
            parentName
        });

    public void AddUserRole(string userId, string roleName)
    {
        var role = _backendService.GetRole(roleName) ?? throw new RoleNotFoundException();
        var user = _backendService.GetUser(userId);
        if (user == null)
        {
            _backendService.CreateUser(new TUser
            {
                UserId = userId,
                RoleNames = new[] { role.Name }
            });
        }
        else
        {
            var list = user.RoleNames.ToList();
            list.Add(role.Name);
            user.RoleNames = list.ToArray();
            _backendService.UpdateUser(user);
        }
    }

    public void AddUserRole(int userId, string roleName) =>
        AddUserRole(userId.ToString(), roleName);

    public void AddUserRole(string userId, TRole role)
    {
        var role1 = Allow(role);
        AddUserRole(userId, role1.Name);
    }

    public void AddUserRole(int userId, TRole role)
    {
        var role1 = Allow(role);
        AddUserRole(userId.ToString(), role1.Name);
    }

    public void AddUserRoles(int userId, string[] roleNames)
    {
        foreach (var roleName in roleNames)
            AddUserRole(userId.ToString(), roleName);
    }

    public void AddUserRoles(string userId, string[] roleNames)
    {
        foreach (var roleName in roleNames)
            AddUserRole(userId, roleName);
    }

    public void AddUserRoles(string userId, TRole[] roles)
    {
        foreach (var role1 in roles)
        {
            var role2 = Allow(role1);
            AddUserRole(userId, role2.Name);
        }
    }

    public void AddUserRoles(int userId, TRole[] roles)
    {
        foreach (var role1 in roles)
        {
            var role2 = Allow(role1);
            AddUserRole(userId.ToString(), role2.Name);
        }
    }

    public void RemoveUserRole(string userId, string roleName)
    {
        var user = _backendService.GetRole(roleName) != null
            ? _backendService.GetUser(userId)
            : throw new RoleNotFoundException();
        var stringList = user != null ? user.RoleNames.ToList() : throw new UserNotFoundException();
        stringList.Remove(roleName);
        user.RoleNames = stringList.ToArray();
        _backendService.UpdateUser(user);
    }

    public void RemoveUserRole(int userId, string roleName) =>
        RemoveUserRole(userId.ToString(), roleName);

    public TRole[] UserRoles(string userId)
    {
        var user = _backendService.GetUser(userId) ?? throw new UserNotFoundException();
        return user.RoleNames.Select(roleName => _backendService.GetRole(roleName) ?? throw new RoleNotFoundException()).ToArray();
    }

    public TRole[] UserRoles(int userId) =>
        UserRoles(userId.ToString());

    public bool UserHasRole(string userId, string roleName) =>
        (_backendService.GetUser(userId) ?? throw new UserNotFoundException()).RoleNames.Contains(roleName);

    public bool UserHasRole(string userId, TRole role) =>
        UserHasRole(userId, role.Name);

    public bool UserHasRole(int userId, string roleName) =>
        UserHasRole(userId.ToString(), roleName);

    public bool UserHasRole(int userId, TRole role) =>
        UserHasRole(userId, role.Name);

    public TUser[] RoleUsers(string roleName)
    {
        var users = _backendService.GetUsers(roleName);
        foreach (var user in users)
        {
            var roleNames = user.RoleNames;
            user.Roles = new List<Role>(roleNames.Length);
            foreach (var t in roleNames)
            {
                var role = _backendService.GetRole(t);
                if (role != null)
                    user.Roles.Add(role);
                else
                    throw new RoleNotFoundException();
            }
        }
        return users;
    }

    public TUser[] RoleUsers(TRole role) =>
        RoleUsers(role.Name);

    public string[] AllowedPermissions(string userId, string resource)
    {
        var roleArray = UserRoles(userId);
        var source = roleArray.SelectMany(role => RecursiveGetRoleResources(role.Name)).ToList();
        var array = source.Where(r => r.Name == resource).ToArray();
        var stringList = new List<string>();
        foreach (var resource1 in array)
        {
            stringList.AddRange(resource1.Permissions.Where(permission => !stringList.Contains(permission.Name))
                .Select(permission => permission.Name));
        }
        return stringList.ToArray();
    }

    public string[] AllowedPermissions(int userId, string resource) =>
        AllowedPermissions(userId.ToString(), resource);

    private IEnumerable<TResource> RecursiveGetRoleResources(string roleName)
    {
        var role = _backendService.GetRole(roleName) ?? throw new RoleNotFoundException();
        var resourceList = role.Resources.ToList();
        resourceList.AddRange(role.Parents.SelectMany(RecursiveGetRoleResources));
        return resourceList.Cast<TResource>().ToArray();
    }

    public bool IsAllowed(string userId, string resource, string permission) =>
        AllowedPermissions(userId, resource).Contains(permission);

    public bool IsAllowed(int userId, string resource, string permission) =>
        AllowedPermissions(userId, resource).Contains(permission);

    public bool IsAllowed(string userId, TResource resource)
    {
        var source = AllowedPermissions(userId, resource.Name);
        return resource.Permissions.All(permission => source.Contains(permission.Name));
    }

    public bool IsAllowed(int userId, TResource resource) =>
        IsAllowed(userId.ToString(), resource);

    public bool IsRoleAllowed(string roleName, string resource, string permission)
    {
        var roleResources = (_backendService.GetRole(roleName) ?? throw new RoleNotFoundException()).Resources;
        var resourceMatch = roleResources.FirstOrDefault(res => res.Name == resource);
        return resourceMatch != null && resourceMatch.Permissions.Any(perm => perm.Name == permission);
    }

    public bool IsRoleAllowed(string roleName, TResource resource)
    {
        var roleResources = (_backendService.GetRole(roleName) ?? throw new RoleNotFoundException()).Resources;
        var resourceMatch = roleResources.FirstOrDefault(res => res.Name == resource.Name);
        return resourceMatch != null && resource.Permissions.All(permission => resourceMatch.Permissions.Any(perm => perm.Name == permission.Name));
    }
}