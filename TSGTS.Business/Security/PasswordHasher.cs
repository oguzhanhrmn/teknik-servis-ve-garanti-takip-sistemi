using System.Security.Cryptography;
using System.Text;

namespace TSGTS.Business.Security;

public static class PasswordHasher
{
    public static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
    }
}
