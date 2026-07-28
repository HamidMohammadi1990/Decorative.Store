using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PropertyCategories.Queries;
using Edition.Application.Features.PropertyCategories.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Property Categories For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("property-category")]
[ApiControllerCategory(ApiControllerCategory.Property)]
[ControllerInfo(PermissionType.ManagePropertyCategory, PermissionType.ManagePropertyGroup)]
public class PropertyCategoryController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPropertyCategory)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPropertyCategoryResponse>>> GetAll(GetAllPropertyCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPropertyCategoryById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPropertyCategoryResponse?>> Get(GetPropertyCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreatePropertyCategory)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePropertyCategoryResponse>> Create(CreatePropertyCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePropertyCategory)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePropertyCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeletePropertyCategory)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePropertyCategoryRequest request)
        => await mediator.Send(request);
}
