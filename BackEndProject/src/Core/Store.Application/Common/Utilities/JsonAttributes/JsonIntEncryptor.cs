using System.Text.Json;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security;

namespace Edition.Application.Common.Utilities.JsonAttributes;

public class JsonIntEncryptor : JsonConverter<int>
{
    public string Key { get; }

    public JsonIntEncryptor(string key)
    {
        Key = key;
    }
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var encrypted = reader.GetString();
        var decrypted = encrypted.Decrypt(Key);
        int.TryParse(decrypted, out var integerValue);
        return integerValue;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        var stringValue = value.ToString();
        var encrypted = stringValue.Encrypt(Key);
        writer.WriteStringValue(encrypted);
    }
}
public class JsonNullableIntEncryptor : JsonConverter<int?>
{
    public string Key { get; }

    public JsonNullableIntEncryptor(string key)
    {
        Key = key;
    }
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        var encrypted = reader.GetString();
        if (string.IsNullOrEmpty(encrypted))
            return null;

        var decrypted = encrypted.Decrypt(Key);
        if (string.IsNullOrEmpty(decrypted))
            return null;

        return int.TryParse(decrypted, out var integerValue) ? integerValue : null;
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var encrypted = value.ToString()!.Encrypt(Key);
        writer.WriteStringValue(encrypted);
    }
}