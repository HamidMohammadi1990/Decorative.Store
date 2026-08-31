using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.UserStoryLikes.Commands;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;

namespace Store.Api.Controllers.v1;

/// <summary>
/// User story likes
/// </summary>
[ApiVersion("1")]
[ControllerName("user-story-like")]
[ApiControllerCategory(ApiControllerCategory.Users)]
public class UserStoryLikeController(ISender mediator) : BaseApiController
{
    [Authorize]
    [HttpPost("toggle")]
    public async Task<ApiResult<ToggleUserStoryLikeResponse>> Toggle(ToggleUserStoryLikeRequest request)
        => await mediator.Send(request);
}
