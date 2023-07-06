using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.DataProvider;

public class AclDbContext : AclDbContext<int, User>
{
    public AclDbContext() { }
    public AclDbContext(DbContextOptions options) : base(options) { }
}

public abstract class AclDbContext<TKey, TUser> : AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
{
    protected AclDbContext() { }
    protected AclDbContext(DbContextOptions options) : base(options) { }
}

public abstract class AclDbContext<TKey, TUser, TRole, TResource> : DbContext
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    protected AclDbContext() { }
    protected AclDbContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<TUser> Users { get; set; } = null!;
    public virtual DbSet<TRole> Roles { get; set; } = null!;
    public virtual DbSet<TResource> Resources { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired();

            entity.HasOne<TRole>().WithMany().HasForeignKey(ut => ut.RoleId).IsRequired();
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();

            entity.HasMany<TResource>().WithOne().HasForeignKey(res => res.RoleId).IsRequired();
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });
    }
}
