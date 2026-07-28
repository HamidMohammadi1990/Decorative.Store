using Asp.Versioning;
using Store.Api.Attributes;
using Edition.Application.Features.ProductFeatureTypes.Queries;
using Edition.Application.Features.ProductFeatureTypes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Feature Types For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-feature-type")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductFeatureType, PermissionType.ManageProductGroup)]
public class ProductFeatureTypeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductFeatureType)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductFeatureTypeResponse>>> GetAll(GetAllProductFeatureTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductFeatureTypeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductFeatureTypeResponse?>> Get(GetProductFeatureTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductFeatureType)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductFeatureTypeResponse>> Create(CreateProductFeatureTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductFeatureType)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductFeatureTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductFeatureType)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductFeatureTypeRequest request)
        => await mediator.Send(request);
}
