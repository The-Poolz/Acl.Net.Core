namespace Acl.Net.Core.Entities;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = Array.Empty<Role>();
    public virtual ICollection<Claim> Claims { get; set; } = Array.Empty<Claim>();
}
