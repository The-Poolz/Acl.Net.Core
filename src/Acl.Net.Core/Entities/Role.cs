namespace Acl.Net.Core.Entities;

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Resource> Resources { get; set; } = Array.Empty<Resource>();

    public virtual Guid UserId { get; set; }
}