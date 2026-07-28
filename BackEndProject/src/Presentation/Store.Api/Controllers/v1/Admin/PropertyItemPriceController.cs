using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PropertyItemPrices.Queries;
using Edition.Application.Features.PropertyItemPrices.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Property Item Prices For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("property-item-price")]
[ApiControllerCategory(ApiControllerCategory.Property)]
[ControllerInfo(PermissionType.ManagePropertyItemPrice, PermissionType.ManagePropertyGroup)]
public class PropertyItemPriceController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPropertyItemPrice)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPropertyItemPriceResponse>>> GetAll(GetAllPropertyItemPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPropertyItemPriceById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPropertyItemPriceResponse?>> Get(GetPropertyItemPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreatePropertyItemPrice)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePropertyItemPriceResponse>> Create(CreatePropertyItemPriceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePropertyItemPrice)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePropertyItemPriceRequest request)
        => await mediator.Send(request);
}
