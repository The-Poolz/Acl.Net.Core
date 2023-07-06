using System.Text;
using Acl.Net.Core.Secrets;
using System.Security.Cryptography;

namespace Acl.Net.Core.Cryptography;

public class UserTokenManager
{
    private readonly ISecretsProvider secretsProvider;

    public UserTokenManager(ISecretsProvider secretsProvider)
    {
        this.secretsProvider = secretsProvider;
    }

    public virtual string GenerateToken<TKey>(TKey userId)
    {
        var key = secretsProvider.Secret;
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length != 32)
        {
            throw new ArgumentException("Secret key from ISecretsProvider must be exactly 32 bytes (256 bits) for AES-256.");
        }

        var iv = GenerateRandomBytes(16);
        var uniqueData = $"{userId}-{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}";
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

    private static byte[] GenerateRandomBytes(int length)
    {
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return randomBytes;
    }
}
