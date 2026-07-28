using System.Security.Cryptography;
using System.Text;
using Store.Common.Utilities;

namespace Store.Common.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(storedHash);

        if (IsBcryptHash(storedHash))
            return BCrypt.Net.BCrypt.Verify(password, storedHash);

        var legacyHash = SecurityUtility.GetSha256Hash(password);
        return FixedTimeEquals(legacyHash, storedHash);
    }

    public bool NeedsRehash(string storedHash)
        => !IsBcryptHash(storedHash);

    private static bool IsBcryptHash(string storedHash)
        => storedHash.StartsWith("$2", StringComparison.Ordinal);

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
               && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
