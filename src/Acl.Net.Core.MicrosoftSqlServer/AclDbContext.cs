using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.MicrosoftSqlServer;

public class AclDbContext : AclDbContext<User>
{
    protected AclDbContext() { }

    public AclDbContext(DbContextOptions options) : base(options) { }
}

public class AclDbContext<TUser> : AclDbContext<TUser, Role, Resource, Permission>
    where TUser : User
{
    protected AclDbContext() { }

    public AclDbContext(DbContextOptions options) : base(options) { }
}

public class AclDbContext<TUser, TRole, TResource, TPermission> : DbContext
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TPermission : Permission
{
    protected AclDbContext() { }
    public AclDbContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<TUser> Users { get; set; } = null!;
    public virtual DbSet<TRole> Roles { get; set; } = null!;
    public virtual DbSet<TResource> Resources { get; set; } = null!;
    public virtual DbSet<TPermission> Permissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.UserId).IsRequired();
            entity.Ignore(u => u.RoleNames);

            entity.HasMany(u => u.Roles).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
            entity.Ignore(r => r.Parents);

            entity.HasMany(r => r.Resources).WithOne().HasForeignKey(res => res.RoleId).IsRequired();
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();

            entity.HasMany(r => r.Permissions).WithOne().HasForeignKey(p => p.ResourceId).IsRequired();
        });

        modelBuilder.Entity<TPermission>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
        });
    }
}
