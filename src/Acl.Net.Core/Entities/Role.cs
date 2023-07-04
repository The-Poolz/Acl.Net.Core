namespace Acl.Net.Core.Entities;

public class Role<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;

    public string Name { get; set; } = null!;

    public virtual TKey UserId { get; set; } = default!;
}
