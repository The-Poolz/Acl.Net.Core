using System.Text;
using EnvironmentManager;
using System.Security.Cryptography;

namespace Acl.Net.Core.Cryptography;

public static class UserTokenManager
{
    private static readonly string Key =
        EnvManager.GetEnvironmentValue<string>("ACL_CRYPTOGRAPHY_KEY", true);

    public static string GenerateToken<TKey>(TKey userId)
    {
        var keyBytes = Encoding.UTF8.GetBytes(Key);
        if (keyBytes.Length != 32)
        {
            throw new ArgumentException("ACL_CRYPTOGRAPHY_KEY must be exactly 32 bytes (256 bits) for AES-256.");
        }

        var iv = GenerateRandomBytes(16);
        var uniqueData = $"{userId}-{Guid.NewGuid()}-{DateTime.Now.Ticks}";
        return EncryptString(uniqueData, keyBytes, iv);
    }

    private static string EncryptString(string plainText, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encrypt, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }
        var encrypted = msEncrypt.ToArray();

        return Convert.ToBase64String(encrypted);
    }

    public static byte[] GenerateRandomBytes(int length)
    {
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return randomBytes;
    }
}
