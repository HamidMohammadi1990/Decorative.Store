using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.WebSiteSettings.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management WebSiteSetting
/// </summary>
[ApiVersion("1")]
[ControllerName("website-setting")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class WebSiteSettingController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("get")]
    public async Task<ApiResult<GetPublicWebSiteSettingResponse>> Get(GetPublicWebSiteSettingRequest request)
        => await mediator.Send(request);
}
