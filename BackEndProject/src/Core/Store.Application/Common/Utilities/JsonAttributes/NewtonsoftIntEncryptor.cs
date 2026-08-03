using Edition.Application.Common.Utilities.Security;
using Newtonsoft.Json;

namespace Edition.Application.Common.Utilities.JsonAttributes;

public sealed class NewtonsoftIntEncryptor(string key) : JsonConverter<int>
{
    public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString().Encrypt(key));
    }

    public override int ReadJson(
        JsonReader reader,
        Type objectType,
        int existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var encrypted = reader.Value?.ToString();
        if (string.IsNullOrEmpty(encrypted))
            return 0;

        var decrypted = encrypted.Decrypt(key);
        return int.TryParse(decrypted, out var result) ? result : 0;
    }
}

public sealed class NewtonsoftNullableIntEncryptor(string key) : JsonConverter<int?>
{
    public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(value.ToString()!.Encrypt(key));
    }

    public override int? ReadJson(
        JsonReader reader,
        Type objectType,
        int? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var encrypted = reader.Value?.ToString();
        if (string.IsNullOrEmpty(encrypted))
            return null;

        var decrypted = encrypted.Decrypt(key);
        return int.TryParse(decrypted, out var result) ? result : null;
    }
}
