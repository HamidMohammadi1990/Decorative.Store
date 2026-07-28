using Store.Common.Enums;

namespace Store.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ApiControllerCategoryAttribute : Attribute
{
    public ApiControllerCategory Category { get; }

    public ApiControllerCategoryAttribute(ApiControllerCategory category)
    {
        Category = category;
    }
}
