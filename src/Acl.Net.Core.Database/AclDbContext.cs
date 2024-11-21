using Microsoft.EntityFrameworkCore;
using Acl.Net.Core.Database.Entities;

namespace Acl.Net.Core.Database;

/// <summary>
/// Represents the application-specific database context for access control lists, utilizing a specific integer key.
/// </summary>
public class AclDbContext : AclDbContext<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AclDbContext"/> class using default role data seeder.
    /// </summary>
    public AclDbContext()
        : base(new RoleDataSeeder())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AclDbContext"/> class with specified options and default role data seeder.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public AclDbContext(DbContextOptions options)
        : base(options, new RoleDataSeeder())
    { }
}

/// <summary>
/// Represents the application-specific database context for access control lists.
/// </summary>
/// <typeparam name="TKey">The type of the key used in the database entities.</typeparam>
public abstract class AclDbContext<TKey> : AclDbContext<TKey, User<TKey>, Role<TKey>, Resource<TKey>>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AclDbContext{TKey}"/> class with the provided data seeder.
    /// </summary>
    /// <param name="seeder">The initial data seeder.</param>
    protected AclDbContext(IInitialDataSeeder<TKey, Role<TKey>> seeder)
        : base(seeder)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AclDbContext{TKey}"/> class with specified options and data seeder.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    /// <param name="seeder">The initial data seeder.</param>
    protected AclDbContext(DbContextOptions options, IInitialDataSeeder<TKey, Role<TKey>> seeder)
        : base(options, seeder)
    { }
}

/// <summary>
/// Represents the application-specific database context for access control lists.
/// </summary>
/// <typeparam name="TKey">The type of the key used in the database entities.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
/// <typeparam name="TRole">The type of the role entity.</typeparam>
/// <typeparam name="TResource">The type of the resource entity.</typeparam>
public abstract class AclDbContext<TKey, TUser, TRole, TResource> : DbContext
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
    where TRole : Role<TKey>
    where TResource : Resource<TKey>
{
    private readonly IInitialDataSeeder<TKey, TRole> _seeder;

    /// <summary>
    /// Initializes a new instance of the <see cref="AclDbContext{TKey, TUser, TRole, TResource}"/> class with the provided data seeder.
    /// </summary>
    /// <param name="seeder">The initial data seeder.</param>
    protected AclDbContext(IInitialDataSeeder<TKey, TRole> seeder)
    {
        _seeder = seeder;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AclDbContext{TKey, TUser, TRole, TResource}"/> class with specified options and data seeder.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    /// <param name="seeder">The initial data seeder.</param>
    protected AclDbContext(DbContextOptions options, IInitialDataSeeder<TKey, TRole> seeder) : base(options)
    {
        _seeder = seeder;
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TUser}"/> representing the users in the database.
    /// </summary>
    public virtual DbSet<TUser> Users { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TRole}"/> representing the roles in the database.
    /// </summary>
    public virtual DbSet<TRole> Roles { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TResource}"/> representing the resources in the database.
    /// </summary>
    public virtual DbSet<TResource> Resources { get; set; } = null!;

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context
    /// Databases (and other extensions) typically define extension methods on this object that allow you to configure aspects of the model that are specific to a given database.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => new { u.Name, u.RoleId });
            entity.Property(u => u.Name).IsRequired();

            entity.HasOne<TRole>().WithMany().HasForeignKey(ut => ut.RoleId).IsRequired();
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();

            entity.HasMany<TResource>().WithOne().HasForeignKey(res => res.RoleId).IsRequired();

            entity.HasData(_seeder.SeedAdminRole());
        });

        modelBuilder.Entity<TResource>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });
    }
}
