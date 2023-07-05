namespace Acl.Net.Core.Secrets;

public interface ISecretsProvider
{
    public string Secret { get; }
}