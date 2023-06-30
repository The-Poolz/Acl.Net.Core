namespace Acl.Net.Core.Entities;

public class Permission
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual Guid ResourceId { get; set; }
}