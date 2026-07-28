using System.ComponentModel.DataAnnotations;
using Store.Common.Resources;

namespace Store.Common.Enums;

public enum ApiControllerCategory
{
    [Display(Name = "ApiControllerCategory_Authentication", ResourceType = typeof(ControllerCategoryResources))]
    Authentication = 1,

    [Display(Name = "ApiControllerCategory_General", ResourceType = typeof(ControllerCategoryResources))]
    General = 2,

    [Display(Name = "ApiControllerCategory_Users", ResourceType = typeof(ControllerCategoryResources))]
    Users = 10,

    [Display(Name = "ApiControllerCategory_Company", ResourceType = typeof(ControllerCategoryResources))]
    Company = 20,

    [Display(Name = "ApiControllerCategory_Catalog", ResourceType = typeof(ControllerCategoryResources))]
    Catalog = 30,

    [Display(Name = "ApiControllerCategory_Blog", ResourceType = typeof(ControllerCategoryResources))]
    Blog = 40,

    [Display(Name = "ApiControllerCategory_Cms", ResourceType = typeof(ControllerCategoryResources))]
    Cms = 50,

    [Display(Name = "ApiControllerCategory_Orders", ResourceType = typeof(ControllerCategoryResources))]
    Orders = 60,

    [Display(Name = "ApiControllerCategory_Wallet", ResourceType = typeof(ControllerCategoryResources))]
    Wallet = 70,

    [Display(Name = "ApiControllerCategory_Financial", ResourceType = typeof(ControllerCategoryResources))]
    Financial = 80,

    [Display(Name = "ApiControllerCategory_Product", ResourceType = typeof(ControllerCategoryResources))]
    Product = 90,

    [Display(Name = "ApiControllerCategory_Location", ResourceType = typeof(ControllerCategoryResources))]
    Location = 100,

    [Display(Name = "ApiControllerCategory_Property", ResourceType = typeof(ControllerCategoryResources))]
    Property = 110,

    [Display(Name = "ApiControllerCategory_ContentPolicy", ResourceType = typeof(ControllerCategoryResources))]
    ContentPolicy = 120,

    [Display(Name = "ApiControllerCategory_AccessControl", ResourceType = typeof(ControllerCategoryResources))]
    AccessControl = 130,

    [Display(Name = "ApiControllerCategory_Comments", ResourceType = typeof(ControllerCategoryResources))]
    Comments = 140,

    [Display(Name = "ApiControllerCategory_Delivery", ResourceType = typeof(ControllerCategoryResources))]
    Delivery = 150,

    [Display(Name = "ApiControllerCategory_PostType", ResourceType = typeof(ControllerCategoryResources))]
    PostType = 160,

    [Display(Name = "ApiControllerCategory_Tags", ResourceType = typeof(ControllerCategoryResources))]
    Tags = 170,

    [Display(Name = "ApiControllerCategory_Localization", ResourceType = typeof(ControllerCategoryResources))]
    Localization = 180
}
