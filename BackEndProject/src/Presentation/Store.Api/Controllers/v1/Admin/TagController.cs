using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Tags.Queries;
using Edition.Application.Features.Tags.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Tag For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("tag")]
[ApiControllerCategory(ApiControllerCategory.Tags)]
[ControllerInfo(PermissionType.ManageTag, PermissionType.ManageTagGroup)]
public class TagController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListTag)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllTagResponse>>> GetAll(GetAllTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetTagById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetTagResponse?>> Get(GetTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateTag)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateTagResponse>> Create(CreateTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateTag)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteTag)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteTagRequest request)
        => await mediator.Send(request);
}
