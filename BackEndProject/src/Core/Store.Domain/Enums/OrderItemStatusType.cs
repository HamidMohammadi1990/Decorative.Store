using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum OrderItemStatusType
{
    [Display(Name = "OrderItemStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "OrderItemStatusType_Rejected", ResourceType = typeof(EnumResources))]
    Rejected = 2,

    [Display(Name = "OrderItemStatusType_RejectedForEdit", ResourceType = typeof(EnumResources))]
    RejectedForEdit = 3,

    [Display(Name = "OrderItemStatusType_AcceptedByCompany", ResourceType = typeof(EnumResources))]
    AcceptedByCompany = 4,

    [Display(Name = "OrderItemStatusType_AwaitingDesignApprovalByUser", ResourceType = typeof(EnumResources))]
    AwaitingDesignApprovalByUser = 5,

    [Display(Name = "OrderItemStatusType_ApprovingDesign", ResourceType = typeof(EnumResources))]
    ApprovingDesign = 6,

    [Display(Name = "OrderItemStatusType_InProgress", ResourceType = typeof(EnumResources))]
    InProgress = 7,

    [Display(Name = "OrderItemStatusType_Completed", ResourceType = typeof(EnumResources))]
    Completed = 8
}
