using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acl.Net.Core.DataProvider;

public class AclDbContext : AclDbContext<int>
{
    public AclDbContext()
        : base(new RoleDataSeeder())
    { }

    public AclDbContext(DbContextOptions options)
        : base(options, new RoleDataSeeder())
    { }
}

public abstract class AclDbContext<TKey> : AclDbContext<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
    protected AclDbContext(IInitialDataSeeder<TKey, Role<TKey>> seeder)
        : base(seeder)
    { }

    protected AclDbContext(DbContextOptions options, IInitialDataSeeder<TKey, Role<TKey>> seeder)
        : base(options, seeder)
    { }
}

public abstract class AclDbContext<TKey, TUser, TRole, TResource> : DbContext
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly IInitialDataSeeder<TKey, TRole> seeder;

    protected AclDbContext(IInitialDataSeeder<TKey, TRole> seeder)
    {
        this.seeder = seeder;
    }

    protected AclDbContext(DbContextOptions options, IInitialDataSeeder<TKey, TRole> seeder) : base(options)
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

            entity.HasData(seeder.SeedAdminRole(), seeder.SeedUserRole());
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });
    }
}
