using System.Security.Cryptography;
using System.Text;

namespace Acl.Net.Core.Cryptography;

public static class MD5Generator
{
    public static string GenerateToken()
    {
        var tokenSource = GenerateTokenSource();

        using var md5 = MD5.Create();

        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(tokenSource));

        return BitConverter.ToString(hash).Replace("-", "").ToUpper();
    }

    private static string GenerateTokenSource()
    {
        var randomNumber = new byte[4];

        RandomNumberGenerator.Fill(randomNumber);

        var randomString = BitConverter.ToUInt32(randomNumber, 0).ToString();

        return randomString + DateTime.UtcNow;
    }
}
