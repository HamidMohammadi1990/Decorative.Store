using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum PropertyType
{
    [Display(Name = "PropertyType_Boolean", ResourceType = typeof(EnumResources))]
    Boolean = 1,

    [Display(Name = "PropertyType_HasParents", ResourceType = typeof(EnumResources))]
    HasParents = 2,

    [Display(Name = "PropertyType_Numeric", ResourceType = typeof(EnumResources))]
    Numeric = 3,

    [Display(Name = "PropertyType_NumericWithItem", ResourceType = typeof(EnumResources))]
    NumericWithItem = 4,

    [Display(Name = "PropertyType_Dimensions", ResourceType = typeof(EnumResources))]
    Dimensions = 5,

    [Display(Name = "PropertyType_Text", ResourceType = typeof(EnumResources))]
    Text = 6,

    [Display(Name = "PropertyType_Select", ResourceType = typeof(EnumResources))]
    Select = 7
}
