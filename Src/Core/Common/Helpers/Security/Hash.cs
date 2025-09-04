using System.Security.Cryptography;
using System.Text;

namespace Barin.Framework.Common.Helpers.Security;

public static class Hash
{
    public static string GetHashCode(byte[] value)
    {
        string base64String;
        using (var sha256 = new SHA256CryptoServiceProvider())
        {
            base64String = Convert.ToBase64String(sha256.ComputeHash(value));
        }
        return base64String;
    }

    public static string GetHashCode(string text)
    {
        return text.GetHashCode().ToString();
    }

    public static string GetHashSha256(string text)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }

    public static ulong GetHashCode(DateTime when)
    {
        ulong kind = (ulong)(int)when.Kind;
        return (kind << 62) | (ulong)when.Ticks;
    }
}
