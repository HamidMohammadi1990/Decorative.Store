using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PostTypes.Queries;
using Edition.Application.Features.PostTypes.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Post Types For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("post-type")]
[ApiControllerCategory(ApiControllerCategory.PostType)]
[ControllerInfo(PermissionType.ManagePostType, PermissionType.ManagePostTypeGroup)]
public class PostTypeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPostType)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPostTypeResponse>>> GetAll(GetAllPostTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPostTypeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPostTypeResponse?>> Get(GetPostTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreatePostType)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePostTypeResponse>> Create(CreatePostTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePostType)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePostTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeletePostType)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePostTypeRequest request)
        => await mediator.Send(request);
}
