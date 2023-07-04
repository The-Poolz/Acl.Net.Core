namespace Acl.Net.Core.Entities;

public class Claim : Claim<int> { }

public class Claim<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;

    public string Token { get; set; } = null!;

    public DateTime DateOfCreation { get; set; }

    public TKey UserId { get; set; } = default!;
}
