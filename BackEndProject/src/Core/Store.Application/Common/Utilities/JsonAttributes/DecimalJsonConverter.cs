using Newtonsoft.Json;
using System.Text.Json;

namespace Edition.Application.Common.Utilities.JsonAttributes;

public class DecimalJsonConverter : System.Text.Json.Serialization.JsonConverter<decimal>
{
    private static bool IsWholeValue(object value)
    {
        if (value is decimal decimalValue)
        {
            return decimalValue == Math.Truncate(decimalValue);
        }
        else if (value is float floatValue)
        {
            return floatValue == Math.Truncate(floatValue);
        }
        else if (value is double doubleValue)
        {
            return doubleValue == Math.Truncate(doubleValue);
        }

        return false;
    }

    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        if (IsWholeValue(value))
        {
            writer.WriteRawValue(JsonConvert.ToString(Convert.ToInt64(value)));
        }
        else
        {
            writer.WriteRawValue(JsonConvert.ToString(value));
        }
    }
}