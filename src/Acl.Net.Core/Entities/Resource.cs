namespace Acl.Net.Core.Entities;

public class Resource
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = Array.Empty<Permission>();

    public virtual Guid RoleId { get; set; }
}