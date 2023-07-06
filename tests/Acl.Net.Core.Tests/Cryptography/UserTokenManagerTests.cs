using Xunit;
using Acl.Net.Core.Cryptography;

namespace Acl.Net.Core.Tests.Cryptography;

public class UserTokenManagerTests
{
    private readonly UserTokenManager userTokenManager;

    public UserTokenManagerTests()
    {
        var secretsProvider = new SecretsProvider();
        userTokenManager = new UserTokenManager(secretsProvider);
    }

    [Fact]
    internal void GenerateToken_ShouldReturnDifferentTokensForDifferentUsers()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");

        var user1Token = userTokenManager.GenerateToken(1);
        var user2Token = userTokenManager.GenerateToken(2);

        Assert.NotEqual(user1Token, user2Token);
    }

    [Fact]
    internal void GenerateToken_ShouldReturnDifferentTokensForSameUser()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");

        var user1Token1 = userTokenManager.GenerateToken(1);
        var user1Token2 = userTokenManager.GenerateToken(1);

        Assert.NotEqual(user1Token1, user1Token2);
    }

    [Fact]
    internal void GenerateToken_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "1234567812345678");

        void TestCode() => userTokenManager.GenerateToken(1);

        Exception ex = Assert.Throws<ArgumentException>(TestCode);
        Assert.Equal("Secret key from ISecretsProvider must be exactly 32 bytes (256 bits) for AES-256.", ex.Message);
    }
}
