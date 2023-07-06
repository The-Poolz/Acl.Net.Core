using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.DataProvider;

public class AclDbContext : AclDbContext<int, User>
{
    public AclDbContext()
        : base(new RoleDataSeeder())
    { }

    public AclDbContext(DbContextOptions options)
        : base(options, new RoleDataSeeder())
    { }
}

public abstract class AclDbContext<TKey, TUser> : AclDbContext<TKey, TUser, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
{
    protected AclDbContext(IInitialDataSeeder<Role<TKey>, TKey> seeder)
        : base(seeder)
    { }

    protected AclDbContext(DbContextOptions options, IInitialDataSeeder<Role<TKey>, TKey> seeder)
        : base(options, seeder)
    { }
}

public abstract class AclDbContext<TKey, TUser, TRole, TResource> : DbContext
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly IInitialDataSeeder<TRole, TKey> seeder;

    protected AclDbContext(IInitialDataSeeder<TRole, TKey> seeder)
    {
        this.seeder = seeder;
    }

    protected AclDbContext(DbContextOptions options, IInitialDataSeeder<TRole, TKey> seeder) : base(options)
    {
        this.seeder = seeder;
    }

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

            entity.HasData(seeder.SeedRoles());
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });
    }
}
