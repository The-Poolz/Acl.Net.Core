namespace Acl.Net.Core.Entities;

public class User : User<int> { }

public class User<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
    public string Name { get; set; } = null!;
    public TKey RoleId { get; set; } = default!;
}
