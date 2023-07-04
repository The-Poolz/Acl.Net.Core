using System.Text;
using EnvironmentManager;
using System.Security.Cryptography;

namespace Acl.Net.Core.Cryptography;

public static class UserTokenManager
{
    /// <summary>
    /// Key needs to be 32 bytes for AES-256.
    /// </summary>
    private static readonly byte[] Key = Encoding.UTF8.GetBytes(
        EnvManager.GetEnvironmentValue<string>("ACL_CRYPTOGRAPHY_KEY", true));

    /// <summary>
    /// // IV (Initialization Vector) needs to be 16 bytes.
    /// If you encrypt different messages with the same key but with different IVs, you will get different outputs each time, even if the messages are the same.
    /// </summary>
    private static readonly byte[] IV = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

    public static string GenerateToken(Guid userId)
    {
        var uniqueData = $"{userId}-{DateTime.Now.Ticks}";
        return EncryptString(uniqueData, Key, IV);
    }

    private static string EncryptString(string plainText, byte[] key, byte[] iv)
    {
        byte[] encrypted;

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encrypt, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }
}
