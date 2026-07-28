using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.WebSiteSettings.Queries;
using Edition.Application.Features.WebSiteSettings.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management WebSiteSetting For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("website-setting")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageWebSiteSetting, PermissionType.ManageCmsGroup)]
public class WebSiteSettingController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.GetWebSiteSettingById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetWebSiteSettingResponse>> Get(GetWebSiteSettingRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateWebSiteSetting)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateWebSiteSettingRequest request)
        => await mediator.Send(request);
}
