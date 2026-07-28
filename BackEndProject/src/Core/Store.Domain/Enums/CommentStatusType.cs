using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum CommentStatusType
{
    [Display(Name = "CommentStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "CommentStatusType_Approved", ResourceType = typeof(EnumResources))]
    Approved = 2,

    [Display(Name = "CommentStatusType_Rejected", ResourceType = typeof(EnumResources))]
    Rejected = 3,

    [Display(Name = "CommentStatusType_Deleted", ResourceType = typeof(EnumResources))]
    Deleted = 4
}
