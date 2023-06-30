using Acl.Net.Core.Entities;
using Acl.Net.Core.Interfaces;

namespace Acl.Net.Core.Providers;

public abstract class Backend<TUser, TRole> : IBackend<TUser, TRole>
    where TUser : User
    where TRole : Role
{
    public TRole AddRoleParents(TRole role, string[] parents)
    {
        role.Parents = Utils.RemoveDuplicates(Utils.JoinArray(role.Parents, parents));
        return role;
    }

    public TRole RemoveRoleParents(TRole role, string[] parents)
    {
        role.Parents = role.Parents.Where(p => !Array.Exists(parents, pe => pe == p)).ToArray();
        return role;
    }

    public abstract void DeletePermission(string permissionName);
    public abstract void DeleteResource(string resourceName);
    public abstract void DeleteRole(TRole role);
    public abstract TRole? GetRole(string roleName);
    public abstract TUser? GetUser(string userId);
    public abstract TRole CreateRole(TRole role);
    public abstract TUser CreateUser(TUser user);
    public abstract TRole UpdateRole(TRole role);
    public abstract TUser UpdateUser(TUser user);
    public abstract TUser[] GetUsers(string roleName);
}