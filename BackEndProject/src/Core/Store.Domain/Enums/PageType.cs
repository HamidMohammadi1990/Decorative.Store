using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum PageType
{
    [Display(Name = "PageType_General", ResourceType = typeof(EnumResources))]
    General = 1,

    [Display(Name = "PageType_Home", ResourceType = typeof(EnumResources))]
    Home = 2,

    [Display(Name = "PageType_CategoryPage", ResourceType = typeof(EnumResources))]
    CategoryPage = 3,

    [Display(Name = "PageType_ProductPage", ResourceType = typeof(EnumResources))]
    ProductPage = 4,

    [Display(Name = "PageType_CheckoutPage", ResourceType = typeof(EnumResources))]
    CheckoutPage = 5
}
