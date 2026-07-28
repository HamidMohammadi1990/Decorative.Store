using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductPropertyPrices.Queries;
using Edition.Application.Features.ProductPropertyPrices.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Property Prices For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-property-price")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductPropertyPrice, PermissionType.ManageProductGroup)]
public class ProductPropertyPriceController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductPropertyPrice)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductPropertyPriceResponse>>> GetAll(GetAllProductPropertyPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductPropertyPriceById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductPropertyPriceResponse?>> Get(GetProductPropertyPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductPropertyPrice)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductPropertyPriceResponse>> Create(CreateProductPropertyPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductPropertyPrice)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductPropertyPriceRequest request)
        => await mediator.Send(request);
}
