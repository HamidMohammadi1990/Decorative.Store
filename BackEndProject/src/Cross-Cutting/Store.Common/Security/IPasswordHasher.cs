namespace Store.Common.Security;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string storedHash);
    bool NeedsRehash(string storedHash);
}
