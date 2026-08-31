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

        var converterAttribute = member.GetCustomAttribute<System.Text.Json.Serialization.JsonConverterAttribute>();
        if (converterAttribute?.ConverterType is null)
            return property;

        var memberType = GetMemberType(member);

        if (Activator.CreateInstance(converterAttribute.ConverterType) is JsonIntEncryptor intEncryptor)
        {
            property.Converter = IsEnumType(memberType)
                ? new NewtonsoftEnumIntEncryptor(intEncryptor.Key, memberType)
                : new NewtonsoftIntEncryptor(intEncryptor.Key);
            return property;
        }

        if (Activator.CreateInstance(converterAttribute.ConverterType) is JsonNullableIntEncryptor nullableEncryptor)
        {
            property.Converter = IsEnumType(memberType)
                ? new NewtonsoftEnumIntEncryptor(nullableEncryptor.Key, memberType)
                : new NewtonsoftNullableIntEncryptor(nullableEncryptor.Key);
            return property;
        }

        return property;
    }

    private static Type GetMemberType(MemberInfo member) =>
        member switch
        {
            PropertyInfo property => property.PropertyType,
            FieldInfo field => field.FieldType,
            _ => throw new NotSupportedException($"Member type '{member.MemberType}' is not supported."),
        };

    private static bool IsEnumType(Type memberType)
    {
        var underlyingType = Nullable.GetUnderlyingType(memberType) ?? memberType;
        return underlyingType.IsEnum;
    }
}
