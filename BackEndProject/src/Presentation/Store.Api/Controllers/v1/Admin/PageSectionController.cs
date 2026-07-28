using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PageSections.Queries;
using Edition.Application.Features.PageSections.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management PageSections For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("page-section")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManagePageSection, PermissionType.ManageCmsGroup)]
public class PageSectionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPageSection)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPageSectionResponse>>> GetAll(GetAllPageSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPageSectionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPageSectionResponse?>> Get(GetPageSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreatePageSection)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePageSectionResponse>> Create(CreatePageSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePageSection)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePageSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeletePageSection)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePageSectionRequest request)
        => await mediator.Send(request);
}
