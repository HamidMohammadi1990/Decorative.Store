using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.ProductWishlists.Queries;
using Edition.Application.Features.ProductWishlists.Commands;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;

namespace Store.Api.Controllers.v1;

/// <summary>
/// User product wishlist
/// </summary>
[Authorize]
[ApiVersion("1")]
[ControllerName("product-wishlist")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductWishlistController(ISender mediator) : BaseApiController
{
    [HttpGet("my")]
    public async Task<ApiResult<GetMyProductWishlistResponse>> My()
        => await mediator.Send(new GetMyProductWishlistRequest());

    [HttpPost("create")]
    public async Task<ApiResult<OperationResult>> Create(CreateProductWishlistRequest request)
        => await mediator.Send(request);

    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductWishlistRequest request)
        => await mediator.Send(request);
}
