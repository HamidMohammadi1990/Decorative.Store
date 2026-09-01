using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.MarketingPromos.Queries;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Marketing promo strip for storefront home page
/// </summary>
[ApiVersion("1")]
[ControllerName("marketing-promo")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class MarketingPromoStoreController(ISender mediator) : BaseApiController
{
    [HttpPost("promo-strip")]
    public async Task<ApiResult<GetMarketingPromoStripResponse>> GetPromoStrip(GetMarketingPromoStripRequest request)
        => await mediator.Send(request);
}
