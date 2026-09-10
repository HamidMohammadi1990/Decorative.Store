using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Pages.Queries;
using Edition.Application.Features.Pages.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Pages For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("page")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManagePage, PermissionType.ManageCmsGroup)]
public class PageController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPage)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPageResponse>>> GetAll(GetAllPageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ListPage)]
    [HttpPost("page-type-guides")]
    public async Task<ApiResult<List<GetPageTypeGuidesResponse>>> GetPageTypeGuides(GetPageTypeGuidesRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPageById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPageResponse?>> Get(GetPageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreatePage)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePageResponse>> Create(CreatePageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePage)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeletePage)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePageRequest request)
        => await mediator.Send(request);
}
