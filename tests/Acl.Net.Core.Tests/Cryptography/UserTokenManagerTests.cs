using Xunit;
using Acl.Net.Core.Cryptography;

namespace Acl.Net.Core.Tests.Cryptography;

public class UserTokenManagerTests
{
    [Fact]
    internal void GenerateToken_ShouldReturnDifferentTokensForDifferentUsers()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");

        var user1Token = UserTokenManager.GenerateToken(1);
        var user2Token = UserTokenManager.GenerateToken(2);

        Assert.NotEqual(user1Token, user2Token);
    }

    [Fact]
    internal void GenerateToken_ShouldReturnDifferentTokensForSameUser()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "12345678123456781234567812345678");

        var user1Token1 = UserTokenManager.GenerateToken(1);
        var user1Token2 = UserTokenManager.GenerateToken(1);

        Assert.NotEqual(user1Token1, user1Token2);
    }

    [Fact]
    internal void GenerateToken_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        Environment.SetEnvironmentVariable("ACL_CRYPTOGRAPHY_KEY", "1234567812345678");

        static void testCode() => UserTokenManager.GenerateToken(1);

        Exception ex = Assert.Throws<ArgumentException>(testCode);
        Assert.Equal("ACL_CRYPTOGRAPHY_KEY must be exactly 32 bytes (256 bits) for AES-256.", ex.Message);
    }
}
