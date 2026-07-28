using Store.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ProductFeatureTypeCode
{
    [Display(Name = "ProductFeatureTypeCode_DisplayOnMenu", ResourceType = typeof(EnumResources))]
    [ProductFeatureTypeValue("true")]
    DisplayOnMenu = 1
}
