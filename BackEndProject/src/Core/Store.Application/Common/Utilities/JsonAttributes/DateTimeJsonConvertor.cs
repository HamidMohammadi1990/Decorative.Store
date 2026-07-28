using System.Text.Json;

namespace Edition.Application.Common.Utilities.JsonAttributes;

public class DateTimeJsonConvertor : System.Text.Json.Serialization.JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteRawValue($"\"{value:yyyy-MM-ddTHH:mm:ss}\"");
    }
}