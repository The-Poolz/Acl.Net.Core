namespace Acl.Net.Core.Entities;

public class User : User<int> { }

public class User<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
    public TKey RoleId { get; set; } = default!;
}
