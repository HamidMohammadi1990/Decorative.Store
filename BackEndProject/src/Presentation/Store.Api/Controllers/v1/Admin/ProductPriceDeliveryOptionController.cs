using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductPriceDeliveryOptions.Queries;
using Edition.Application.Features.ProductPriceDeliveryOptions.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Price Delivery Option For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-price-delivery-option")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductPriceDeliveryOption, PermissionType.ManageProductGroup)]
public class ProductPriceDeliveryOptionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductPriceDeliveryOption)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductPriceDeliveryOptionResponse>>> GetAll(GetAllProductPriceDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductPriceDeliveryOptionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductPriceDeliveryOptionResponse?>> Get(GetProductPriceDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductPriceDeliveryOption)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductPriceDeliveryOptionResponse>> Create(CreateProductPriceDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductPriceDeliveryOption)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductPriceDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductPriceDeliveryOption)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductPriceDeliveryOptionRequest request)
        => await mediator.Send(request);
}
