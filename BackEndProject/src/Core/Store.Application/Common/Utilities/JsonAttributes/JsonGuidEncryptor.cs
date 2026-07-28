using System.Text.Json;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security;

namespace Edition.Application.Common.Utilities.JsonAttributes;

public class JsonGuidEncryptor : JsonConverter<Guid>
{
    public string Key { get; }

    public JsonGuidEncryptor(string key) => Key = key;

    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var encrypted = reader.GetString();
        var decrypted = encrypted.Decrypt(Key);
        return Guid.TryParse(decrypted, out var value) ? value : Guid.Empty;
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString().Encrypt(Key));
}

public class JsonNullableGuidEncryptor : JsonConverter<Guid?>
{
    public string Key { get; }

    public JsonNullableGuidEncryptor(string key) => Key = key;

    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var encrypted = reader.GetString();
        if (string.IsNullOrWhiteSpace(encrypted))
            return null;

        var decrypted = encrypted.Decrypt(Key);
        return Guid.TryParse(decrypted, out var value) ? value : null;
    }

    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value.ToString().Encrypt(Key));
    }
}
