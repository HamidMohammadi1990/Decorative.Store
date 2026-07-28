using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

/// <summary>
/// Standard content-policy query actions.
/// </summary>
public enum ContentPolicyQueryAction
{
    /// <summary>Wildcard — policy applies to every action.</summary>
    [Display(Name = "ContentPolicyQueryAction_All", ResourceType = typeof(EnumResources))]
    All = 0,

    /// <summary>Single-entity read/update/delete (resource pipeline).</summary>
    [Display(Name = "ContentPolicyQueryAction_Get", ResourceType = typeof(EnumResources))]
    Get = 1,

    [Display(Name = "ContentPolicyQueryAction_GetAll", ResourceType = typeof(EnumResources))]
    GetAll = 2,

    [Display(Name = "ContentPolicyQueryAction_Search", ResourceType = typeof(EnumResources))]
    Search = 3,

    [Display(Name = "ContentPolicyQueryAction_GetUserAddresses", ResourceType = typeof(EnumResources))]
    GetUserAddresses = 4
}
