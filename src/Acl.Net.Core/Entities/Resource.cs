namespace Acl.Net.Core.Entities;

public class Resource<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;

    public string Name { get; set; } = null!;

    public virtual TKey RoleId { get; set; } = default!;
}
