using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Store.WebFramework.Api;
using Store.Api.Modules;
using Store.Common.Extensions;

namespace Store.Api.Filters;

/// <summary>
/// Check User Has Permission
/// </summary>
public class PermissionAuthorizeAttribute
    (Lazy<IAccountingService> accountingService)
    : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var isAdminController = typeof(BaseApiAdminController).IsAssignableFrom(actionDescriptor.ControllerTypeInfo);

        if (!isAdminController)
            return;

        if (!(context.HttpContext.User.Identity?.IsAuthenticated ?? false))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
        if (claimsIdentity!.Claims?.Any() != true)
        {
            context.Result = new NotFoundObjectResult("InvalidRequest");
            return;
        }

        if (!AdminPermissionDiscovery.TryGetMetadata(actionDescriptor.ControllerTypeInfo, out var metadata))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!AdminActionPermissionResolver.HasActionPermission(actionDescriptor.MethodInfo))
        {
            context.Result = new ForbidResult();
            return;
        }

        var permissionType = AdminActionPermissionResolver.Resolve(actionDescriptor.MethodInfo, metadata);
        var userId = claimsIdentity.GetUserId<int>();
        var hasPermission = await accountingService.Value.HasPermissionAsync(userId, permissionType);
        if (!hasPermission)
            context.Result = new ForbidResult();
    }
}
