using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.Orders.Queries;
using Edition.Application.Features.Orders.Commands;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Orders
/// </summary>
[ApiVersion("1")]
[ControllerName("order")]
[ApiControllerCategory(ApiControllerCategory.Orders)]
public class OrderController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("checkout")]
    public async Task<ApiResult<CheckoutOrderResponse>> Checkout(CheckoutOrderRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("validate-discount")]
    public async Task<ApiResult<ValidateDiscountOrderResponse>> ValidateDiscount(ValidateDiscountOrderRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("payment")]
    public async Task<ApiResult<PaymentOrderResponse>> Payment(PaymentOrderRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("payment/verify")]
    public async Task<ApiResult<VerifyPaymentOrderResponse>> VerifyPayment(VerifyPaymentOrderRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpGet("cart")]
    public async Task<ApiResult<GetCartResponse>> GetCart()
        => await mediator.Send(new GetCartRequest());

    [Authorize]
    [HttpPost("cart/items")]
    public async Task<ApiResult<GetCartResponse>> AddCartItem(AddCartItemRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("purchase")]
    public async Task<ApiResult<PurchaseOrderResponse>> Purchase(PurchaseOrderRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpDelete("discount")]
    public async Task<ApiResult<RemoveOrderDiscountResponse>> RemoveDiscount()
        => await mediator.Send(new RemoveOrderDiscountRequest());

    [Authorize]
    [HttpDelete("item")]
    public async Task<ApiResult<RemoveOrderItemResponse>> RemoveItem(RemoveOrderItemRequest request)
        => await mediator.Send(request);
}