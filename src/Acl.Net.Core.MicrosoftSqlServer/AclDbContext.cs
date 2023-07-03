using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.MicrosoftSqlServer;

public class AclDbContext : AclDbContext<User>
{
    protected AclDbContext() { }

    public AclDbContext(DbContextOptions options) : base(options) { }
}

public class AclDbContext<TUser> : AclDbContext<TUser, Role, Resource, Claim>
    where TUser : User
{
    protected AclDbContext() { }

    public AclDbContext(DbContextOptions options) : base(options) { }
}

public class AclDbContext<TUser, TRole, TResource, TClaim> : DbContext
    where TUser : User
    where TRole : Role
    where TResource : Resource
    where TClaim : Claim
{
    protected AclDbContext() { }
    public AclDbContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<TUser> Users { get; set; } = null!;
    public virtual DbSet<TRole> Roles { get; set; } = null!;
    public virtual DbSet<TResource> Resources { get; set; } = null!;
    public virtual DbSet<TClaim> Claims { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.UserId).IsRequired();

            entity.HasMany(u => u.Roles).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            entity.HasMany(u => u.Claims).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();

            entity.HasMany(r => r.Resources).WithOne().HasForeignKey(res => res.RoleId).IsRequired();
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });

        modelBuilder.Entity<TClaim>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Token).IsRequired();
        });
    }
}
