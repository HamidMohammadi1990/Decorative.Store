using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

/// <summary>
/// Logical OR-group for content policy rules. Rules in the same group are combined with AND.
/// </summary>
public enum ContentPolicyRuleGroup
{
    [Display(Name = "ContentPolicyRuleGroup_Group0", ResourceType = typeof(EnumResources))]
    Group0 = 0,

    [Display(Name = "ContentPolicyRuleGroup_Group1", ResourceType = typeof(EnumResources))]
    Group1 = 1,

    [Display(Name = "ContentPolicyRuleGroup_Group2", ResourceType = typeof(EnumResources))]
    Group2 = 2,

    [Display(Name = "ContentPolicyRuleGroup_Group3", ResourceType = typeof(EnumResources))]
    Group3 = 3,

    [Display(Name = "ContentPolicyRuleGroup_Group4", ResourceType = typeof(EnumResources))]
    Group4 = 4,

    [Display(Name = "ContentPolicyRuleGroup_Group5", ResourceType = typeof(EnumResources))]
    Group5 = 5,

    [Display(Name = "ContentPolicyRuleGroup_Group6", ResourceType = typeof(EnumResources))]
    Group6 = 6,

    [Display(Name = "ContentPolicyRuleGroup_Group7", ResourceType = typeof(EnumResources))]
    Group7 = 7,

    [Display(Name = "ContentPolicyRuleGroup_Group8", ResourceType = typeof(EnumResources))]
    Group8 = 8,

    [Display(Name = "ContentPolicyRuleGroup_Group9", ResourceType = typeof(EnumResources))]
    Group9 = 9
}
