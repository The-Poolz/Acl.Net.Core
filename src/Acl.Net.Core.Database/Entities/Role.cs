namespace Acl.Net.Core.Database.Entities;

/// <summary>
/// Represents a role within the Access Control List (ACL) system.
/// </summary>
public class Role : Role<int>;

/// <summary>
/// Represents a role within the Access Control List (ACL) system, with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key that represents the unique identifier for the role.</typeparam>
public class Role<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    /// <value>The unique identifier for the role.</value>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    /// <value>The name of the role.</value>
    public string Name { get; set; } = null!;
}
