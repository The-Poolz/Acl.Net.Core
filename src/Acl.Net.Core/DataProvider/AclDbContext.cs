using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.DataProvider;

public class AclDbContext<TKey, TUser> : AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>, Claim<TKey>>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
{
    protected AclDbContext() { }
    public AclDbContext(DbContextOptions options) : base(options) { }
}

public class AclDbContext<TKey, TUser, TRole, TResource, TClaim> : DbContext
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
    where TClaim : Claim<TKey>
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

            entity.HasMany<TRole>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            entity.HasMany<TClaim>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
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

        modelBuilder.Entity<TClaim>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Token).IsRequired();
            entity.Property(c => c.DateOfCreation).IsRequired();
        });
    }
}
