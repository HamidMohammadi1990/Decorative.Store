using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductPrices.Queries;
using Edition.Application.Features.ProductPrices.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Prices For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-price")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductPrice, PermissionType.ManageProductGroup)]
public class ProductPriceController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductPrice)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductPriceResponse>>> GetAll(GetAllProductPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductPriceById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductPriceResponse?>> Get(GetProductPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductPrice)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductPriceResponse>> Create(CreateProductPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductPrice)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductPriceRequest request)
        => await mediator.Send(request);
}
