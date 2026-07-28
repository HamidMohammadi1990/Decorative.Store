using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ContentPolicyOperator
{
    [Display(Name = "ContentPolicyOperator_Equals", ResourceType = typeof(EnumResources))]
    Equals = 1,

    [Display(Name = "ContentPolicyOperator_NotEquals", ResourceType = typeof(EnumResources))]
    NotEquals = 2,

    [Display(Name = "ContentPolicyOperator_In", ResourceType = typeof(EnumResources))]
    In = 3,

    [Display(Name = "ContentPolicyOperator_NotIn", ResourceType = typeof(EnumResources))]
    NotIn = 4,

    [Display(Name = "ContentPolicyOperator_Exists", ResourceType = typeof(EnumResources))]
    Exists = 5,

    [Display(Name = "ContentPolicyOperator_NotExists", ResourceType = typeof(EnumResources))]
    NotExists = 6,

    [Display(Name = "ContentPolicyOperator_Contains", ResourceType = typeof(EnumResources))]
    Contains = 7,

    [Display(Name = "ContentPolicyOperator_NotContains", ResourceType = typeof(EnumResources))]
    NotContains = 8,

    [Display(Name = "ContentPolicyOperator_StartsWith", ResourceType = typeof(EnumResources))]
    StartsWith = 9,

    [Display(Name = "ContentPolicyOperator_EndsWith", ResourceType = typeof(EnumResources))]
    EndsWith = 10,

    [Display(Name = "ContentPolicyOperator_GreaterThan", ResourceType = typeof(EnumResources))]
    GreaterThan = 11,

    [Display(Name = "ContentPolicyOperator_GreaterThanOrEqual", ResourceType = typeof(EnumResources))]
    GreaterThanOrEqual = 12,

    [Display(Name = "ContentPolicyOperator_LessThan", ResourceType = typeof(EnumResources))]
    LessThan = 13,

    [Display(Name = "ContentPolicyOperator_LessThanOrEqual", ResourceType = typeof(EnumResources))]
    LessThanOrEqual = 14,

    [Display(Name = "ContentPolicyOperator_IsNull", ResourceType = typeof(EnumResources))]
    IsNull = 15,

    [Display(Name = "ContentPolicyOperator_IsNotNull", ResourceType = typeof(EnumResources))]
    IsNotNull = 16,

    [Display(Name = "ContentPolicyOperator_Between", ResourceType = typeof(EnumResources))]
    Between = 17,

    [Display(Name = "ContentPolicyOperator_NotBetween", ResourceType = typeof(EnumResources))]
    NotBetween = 18,

    [Display(Name = "ContentPolicyOperator_CountEquals", ResourceType = typeof(EnumResources))]
    CountEquals = 19,

    [Display(Name = "ContentPolicyOperator_CountNotEquals", ResourceType = typeof(EnumResources))]
    CountNotEquals = 20,

    [Display(Name = "ContentPolicyOperator_CountGreaterThan", ResourceType = typeof(EnumResources))]
    CountGreaterThan = 21,

    [Display(Name = "ContentPolicyOperator_CountGreaterThanOrEqual", ResourceType = typeof(EnumResources))]
    CountGreaterThanOrEqual = 22,

    [Display(Name = "ContentPolicyOperator_CountLessThan", ResourceType = typeof(EnumResources))]
    CountLessThan = 23,

    [Display(Name = "ContentPolicyOperator_CountLessThanOrEqual", ResourceType = typeof(EnumResources))]
    CountLessThanOrEqual = 24
}
