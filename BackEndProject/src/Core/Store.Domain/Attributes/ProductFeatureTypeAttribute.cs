namespace Store.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ProductFeatureTypeValueAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}