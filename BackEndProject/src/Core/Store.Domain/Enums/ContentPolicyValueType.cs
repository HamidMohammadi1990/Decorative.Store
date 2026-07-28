using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ContentPolicyValueType
{
    [Display(Name = "ContentPolicyValueType_Literal", ResourceType = typeof(EnumResources))]
    Literal = 1,

    [Display(Name = "ContentPolicyValueType_Context", ResourceType = typeof(EnumResources))]
    Context = 2
}
