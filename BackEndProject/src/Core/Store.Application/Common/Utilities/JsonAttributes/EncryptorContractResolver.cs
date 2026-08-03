using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Edition.Application.Common.Utilities.JsonAttributes;

/// <summary>
/// Applies System.Text.Json encryptor attributes during Newtonsoft API serialization.
/// </summary>
public sealed class EncryptorContractResolver : CamelCasePropertyNamesContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        var converterAttribute = member.GetCustomAttribute<JsonConverterAttribute>();
        if (converterAttribute?.ConverterType is null)
            return property;

        if (Activator.CreateInstance(converterAttribute.ConverterType) is JsonIntEncryptor intEncryptor)
        {
            property.Converter = new NewtonsoftIntEncryptor(intEncryptor.Key);
            return property;
        }

        if (Activator.CreateInstance(converterAttribute.ConverterType) is JsonNullableIntEncryptor nullableEncryptor)
        {
            property.Converter = new NewtonsoftNullableIntEncryptor(nullableEncryptor.Key);
            return property;
        }

        return property;
    }
}
