namespace Acl.Net.Core.Database.Entities;

/// <summary>
/// Represents a specific resource in the system.
/// </summary>
public class Resource : Resource<int> { }

/// <summary>
/// Represents a specific resource in the system, identified by a generic key type.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the resource.</typeparam>
public class Resource<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for this resource.
    /// </summary>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of this resource.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role identifier associated with this resource.
    /// </summary>
    public virtual TKey RoleId { get; set; } = default!;
}
