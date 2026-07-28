using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum UserSessionRevokeReason
{
    [Display(Name = "UserSessionRevokeReason_SignOut", ResourceType = typeof(EnumResources))]
    SignOut = 1,

    [Display(Name = "UserSessionRevokeReason_PasswordChanged", ResourceType = typeof(EnumResources))]
    PasswordChanged = 2,

    [Display(Name = "UserSessionRevokeReason_RevokedByUser", ResourceType = typeof(EnumResources))]
    RevokedByUser = 3,

    [Display(Name = "UserSessionRevokeReason_RevokedOtherSessions", ResourceType = typeof(EnumResources))]
    RevokedOtherSessions = 4,

    [Display(Name = "UserSessionRevokeReason_SessionLimitExceeded", ResourceType = typeof(EnumResources))]
    SessionLimitExceeded = 5,

    [Display(Name = "UserSessionRevokeReason_SecurityStampChanged", ResourceType = typeof(EnumResources))]
    SecurityStampChanged = 6
}
