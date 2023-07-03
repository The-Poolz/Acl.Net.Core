using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.MicrosoftSqlServer;

public class MsSql : MsSql<User>
{
    public MsSql(AclDbContext context)
        : base(context)
    { }
}

public class MsSql<TUser> : MsSql<TUser, Role, Resource>
    where TUser : User
{
    public MsSql(AclDbContext<TUser> context)
        : base(context)
    { }
}

public class MsSql<TUser, TRole, TResource>
    where TUser : User
    where TRole : Role
    where TResource : Resource
{
    private readonly AclDbContext<TUser, TRole, TResource> _context;

    public MsSql(AclDbContext<TUser, TRole, TResource> context)
    {
        _context = context;
    }

    public TRole CreateRole(TRole role)
    {
        _context.Roles.Add(role);
        _context.SaveChanges();
        return role;
    }

    public TRole? GetRole(string roleName) =>
        _context.Roles.Include(r => r.Resources).SingleOrDefault(r => r.Name == roleName);

    public TRole UpdateRole(TRole role)
    {
        _context.Roles.Update(role);
        _context.SaveChanges();
        return role;
    }

    public void DeleteRole(TRole role)
    {
        var toDelete = _context.Roles.SingleOrDefault(r => r.Id == role.Id);
        if (toDelete == null) return;
        _context.Roles.Remove(toDelete);
        _context.SaveChanges();
    }

    public TUser CreateUser(TUser user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public TUser? GetUser(string userId) =>
        _context.Users.SingleOrDefault(u => u.UserId == userId);

    public TUser UpdateUser(TUser user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return user;
    }

    public void DeleteResource(string resourceName)
    {
        var roles = _context.Roles.Include(r => r.Resources).ToList();
        foreach (var role in roles)
        {
            role.Resources = role.Resources.Where(r => r.Name != resourceName).ToArray();
        }
        _context.SaveChanges();
    }
}