namespace Acl.Net.Core.Entities;

public class Claim
{
    public Guid Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime DateOfCreation { get; set; }
    public Guid UserId { get; set; }
}