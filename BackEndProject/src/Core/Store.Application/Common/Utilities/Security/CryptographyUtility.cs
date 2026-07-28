using System.Text;
using System.Security.Cryptography;
using Store.Common.Security;

namespace Edition.Application.Common.Utilities.Security;

public static class CryptographyUtility
{
    private const string AesPrefix = "A1:";

    public static string Encrypt(this string value)
        => Encrypt(value, SecurityKeyRegistry.GeneralKey);

    public static string Decrypt(this string value)
        => Decrypt(value, SecurityKeyRegistry.GeneralKey);

    public static string Encrypt(this string value, string hashKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashKey);

        var key = DeriveAesKey(hashKey);
        var plainBytes = Encoding.UTF8.GetBytes(value);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        var payload = new byte[aes.IV.Length + cipherBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, payload, 0, aes.IV.Length);
        Buffer.BlockCopy(cipherBytes, 0, payload, aes.IV.Length, cipherBytes.Length);

        return AesPrefix + Convert.ToBase64String(payload);
    }

    public static string Decrypt(this string value, string hashKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashKey);

        if (value.StartsWith(AesPrefix, StringComparison.Ordinal))
            return DecryptAes(value[AesPrefix.Length..], hashKey);

        return DecryptLegacyTripleDes(value, hashKey);
    }

    private static string DecryptAes(string payload, string hashKey)
    {
        var key = DeriveAesKey(hashKey);
        var payloadBytes = Convert.FromBase64String(payload);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var ivLength = aes.BlockSize / 8;
        if (payloadBytes.Length <= ivLength)
            throw new CryptographicException("Invalid encrypted payload.");

        var iv = new byte[ivLength];
        var cipherBytes = new byte[payloadBytes.Length - ivLength];
        Buffer.BlockCopy(payloadBytes, 0, iv, 0, ivLength);
        Buffer.BlockCopy(payloadBytes, ivLength, cipherBytes, 0, cipherBytes.Length);

        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    private static string DecryptLegacyTripleDes(string value, string hashKey)
    {
        byte[] decrypted;
        using (var tdes = TripleDES.Create())
        {
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            var key = Encoding.UTF8.GetBytes(hashKey);
            var decryptor = tdes.CreateDecryptor(key, tdes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                var cipherBytes = Convert.FromBase64String(value);
                cs.Write(cipherBytes, 0, cipherBytes.Length);
            }
            decrypted = ms.ToArray();
        }

        return Encoding.UTF8.GetString(decrypted);
    }

    private static byte[] DeriveAesKey(string hashKey)
        => SHA256.HashData(Encoding.UTF8.GetBytes(hashKey));
}
