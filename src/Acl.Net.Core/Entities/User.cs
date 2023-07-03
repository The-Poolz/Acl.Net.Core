namespace Acl.Net.Core.Entities;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = Array.Empty<Role>();

    /// <summary>
    /// Returns the <see cref="UserId"/> for this user.
    /// </summary>
    public override string ToString() =>
        string.IsNullOrWhiteSpace(UserId) ? UserId : string.Empty;
}