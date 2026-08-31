using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.UserStoryComments.Queries;
using Edition.Application.Features.UserStoryComments.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// User story comment management for admin
/// </summary>
[ApiVersion("1")]
[ControllerName("user-story-comment")]
[ApiControllerCategory(ApiControllerCategory.Users)]
[ControllerInfo(PermissionType.ManageUserStoryComment, PermissionType.ManageUsersGroup)]
public class UserStoryCommentController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListUserStoryComment)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllUserStoryCommentResponse>>> GetAll(GetAllUserStoryCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ApproveUserStoryComment)]
    [HttpPost("approve")]
    public async Task<ApiResult<OperationResult>> Approve(ApproveUserStoryCommentRequest request)
        => await mediator.Send(request);
}
