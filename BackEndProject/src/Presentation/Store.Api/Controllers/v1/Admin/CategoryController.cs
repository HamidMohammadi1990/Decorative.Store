using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Categories.Queries;
using Edition.Application.Features.Categories.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Categories For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("category")]
[ApiControllerCategory(ApiControllerCategory.Catalog)]
[ControllerInfo(PermissionType.ManageCategory, PermissionType.ManageCategoryGroup)]
public class CategoryController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCategory)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCategoryResponse>>> GetAll(GetAllCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCategoryById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCategoryResponse?>> Get(GetCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateCategory)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCategoryResponse>> Create(CreateCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateCategory)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteCategory)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteCategoryRequest request)
        => await mediator.Send(request);
}
