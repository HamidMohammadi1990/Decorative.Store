internal static class ProductPropertyReferenceRules
{
    internal static int? NormalizePropertyItemId(int? propertyItemId)
        => propertyItemId is null or 0 ? null : propertyItemId;
}
