using Store.Domain.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Common.Extensions;

public static class EnumExtensions
{
    public static string? ToValue(this ProductFeatureTypeCode featureType)
    {
        var type = featureType.GetType();
        var member = type.GetMember(featureType.ToString()).FirstOrDefault();
        if (member is null)
            return null;

        var attribute = member.GetCustomAttributes(typeof(ProductFeatureTypeValueAttribute), false)
                              .FirstOrDefault() as ProductFeatureTypeValueAttribute;
        return attribute?.Value;
    }
}