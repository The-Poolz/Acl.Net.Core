using Acl.Net.Core.Secrets;

namespace Acl.Net.Core.Tests.Cryptography;

internal class SecretsProvider : ISecretsProvider
{
    public string Secret => Environment.GetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY") ?? "1234567812345678";
}