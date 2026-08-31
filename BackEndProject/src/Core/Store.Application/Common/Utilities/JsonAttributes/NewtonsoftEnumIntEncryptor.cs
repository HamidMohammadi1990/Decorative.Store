using Edition.Application.Common.Utilities.Security;
using Newtonsoft.Json;

namespace Edition.Application.Common.Utilities.JsonAttributes;

/// <summary>
/// Encrypts int-backed enum values for Newtonsoft serialization.
/// </summary>
public sealed class NewtonsoftEnumIntEncryptor(string key, Type enumType) : JsonConverter
{
    private readonly Type _enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

    public override bool CanConvert(Type objectType)
    {
        var type = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return type == _enumType;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var intValue = Convert.ToInt32(value);
        writer.WriteValue(intValue.ToString().Encrypt(key));
    }

    public override object? ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var encrypted = reader.Value?.ToString();
        if (string.IsNullOrEmpty(encrypted))
            return null;

        var decrypted = encrypted.Decrypt(key);
        if (!int.TryParse(decrypted, out var result))
            return null;

        return Enum.ToObject(_enumType, result);
    }
}
