using System.ComponentModel.DataAnnotations;
using Store.Common.Resources;

namespace Store.Common.Enums;

public enum OperationStatusCode
{
    [Display(Name = "OperationStatusCode_OK", ResourceType = typeof(EnumResources))]
    OK = 200,

    [Display(Name = "OperationStatusCode_ServerError", ResourceType = typeof(EnumResources))]
    ServerError = 500,

    [Display(Name = "OperationStatusCode_BadRequest", ResourceType = typeof(EnumResources))]
    BadRequest = 400,

    [Display(Name = "OperationStatusCode_NotFound", ResourceType = typeof(EnumResources))]
    NotFound = 404,

    [Display(Name = "OperationStatusCode_UnAuthorized", ResourceType = typeof(EnumResources))]
    UnAuthorized = 401
}
