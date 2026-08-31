using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.UserStoryComments.Queries;
using Edition.Application.Features.UserStoryComments.Commands;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// User story comments
/// </summary>
[ApiVersion("1")]
[ControllerName("user-story-comment")]
[ApiControllerCategory(ApiControllerCategory.Users)]
public class UserStoryCommentController(ISender mediator) : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchUserStoryCommentResponse>>> Search(SearchUserStoryCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateUserStoryCommentResponse>> Create(CreateUserStoryCommentRequest request)
        => await mediator.Send(request);
}
