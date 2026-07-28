using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.SubCategories.Queries;
using Edition.Application.Features.SubCategories.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management SubCategories For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("sub-category")]
[ApiControllerCategory(ApiControllerCategory.Catalog)]
[ControllerInfo(PermissionType.ManageSubCategory, PermissionType.ManageSubCategoryGroup)]
public class SubCategoryController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListSubCategory)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllSubCategoryResponse>>> GetAll(GetAllSubCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetSubCategoryById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetSubCategoryResponse?>> Get(GetSubCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateSubCategory)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateSubCategoryResponse>> Create(CreateSubCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateSubCategory)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateSubCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteSubCategory)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteSubCategoryRequest request)
        => await mediator.Send(request);
}
