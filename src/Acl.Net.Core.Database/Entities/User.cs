namespace Acl.Net.Core.Database.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User : User<int>;

/// <summary>
/// Represents a user in the system, identified by a generic key type.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the user.</typeparam>
public class User<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for this user.
    /// </summary>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of this user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role identifier associated with this user.
    /// </summary>
    public TKey RoleId { get; set; } = default!;
}
