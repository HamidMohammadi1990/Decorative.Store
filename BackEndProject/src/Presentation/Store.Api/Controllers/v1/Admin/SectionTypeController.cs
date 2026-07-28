using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.SectionTypes.Queries;
using Edition.Application.Features.SectionTypes.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management SectionTypes For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("section-type")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageSectionType, PermissionType.ManageCmsGroup)]
public class SectionTypeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListSectionType)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllSectionTypeResponse>>> GetAll(GetAllSectionTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetSectionTypeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetSectionTypeResponse?>> Get(GetSectionTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateSectionType)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateSectionTypeResponse>> Create(CreateSectionTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateSectionType)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateSectionTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteSectionType)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteSectionTypeRequest request)
        => await mediator.Send(request);
}
