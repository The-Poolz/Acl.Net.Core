namespace Acl.Net.Core.Entities;

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual Guid UserId { get; set; }
}
