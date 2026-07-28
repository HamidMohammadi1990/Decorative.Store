using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ForgetPasswordOptionType
{
    [Display(Name = "ForgetPasswordOptionType_Message", ResourceType = typeof(EnumResources))]
    Message = 1,

    [Display(Name = "ForgetPasswordOptionType_Email", ResourceType = typeof(EnumResources))]
    Email = 2
}
