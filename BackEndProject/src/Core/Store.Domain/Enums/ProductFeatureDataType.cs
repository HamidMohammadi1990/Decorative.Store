using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ProductFeatureDataType
{
    [Display(Name = "ProductFeatureDataType_String", ResourceType = typeof(EnumResources))]
    String,

    [Display(Name = "ProductFeatureDataType_Integer", ResourceType = typeof(EnumResources))]
    Integer,

    [Display(Name = "ProductFeatureDataType_Decimal", ResourceType = typeof(EnumResources))]
    Decimal,

    [Display(Name = "ProductFeatureDataType_Boolean", ResourceType = typeof(EnumResources))]
    Boolean,

    [Display(Name = "ProductFeatureDataType_DateTime", ResourceType = typeof(EnumResources))]
    DateTime,

    [Display(Name = "ProductFeatureDataType_Float", ResourceType = typeof(EnumResources))]
    Float,

    [Display(Name = "ProductFeatureDataType_Long", ResourceType = typeof(EnumResources))]
    Long,

    [Display(Name = "ProductFeatureDataType_Complex", ResourceType = typeof(EnumResources))]
    Complex
}
