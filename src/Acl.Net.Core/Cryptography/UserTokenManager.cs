using System.Security.Cryptography;
using System.Text;

namespace Acl.Net.Core.Cryptography;

public class UserTokenManager
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("Insert a 32 length string here."); // Key needs to be 32 bytes for AES-256.
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("Insert a 16 length string here."); // IV needs to be 16 bytes.

    public string GenerateToken(string userId)
    {
        var uniqueData = $"{userId}-{DateTime.Now.Ticks}";
        return EncryptString(uniqueData, Key, IV);
    }

    private string EncryptString(string plainText, byte[] key, byte[] iv)
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
