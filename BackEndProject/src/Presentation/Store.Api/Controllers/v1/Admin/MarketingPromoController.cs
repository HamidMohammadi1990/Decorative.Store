using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.MarketingPromos.Queries;
using Edition.Application.Features.MarketingPromos.Commands;
using Edition.Application.Features.NewsletterSubscribers.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management marketing promo items for admin
/// </summary>
[ApiVersion("1")]
[ControllerName("marketing-promo")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageMarketingPromo, PermissionType.ManageMarketingPromoGroup)]
public class MarketingPromoController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListMarketingPromo)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllMarketingPromoResponse>>> GetAll(GetAllMarketingPromoRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetMarketingPromoById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetMarketingPromoResponse?>> Get(GetMarketingPromoRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateMarketingPromo)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateMarketingPromoResponse>> Create(CreateMarketingPromoRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateMarketingPromo)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateMarketingPromoRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteMarketingPromo)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteMarketingPromoRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateMarketingPromo)]
    [HttpPost("upload-image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ApiResult<UploadMarketingPromoImageResponse>> UploadImage([FromForm] IFormFile image)
        => await mediator.Send(new UploadMarketingPromoImageCommand(image));

    [ActionInfo(PermissionType.UpdateMarketingPromoDisclaimer)]
    [HttpPut("update-disclaimer")]
    public async Task<ApiResult<OperationResult>> UpdateDisclaimer(UpdateMarketingStripDisclaimerRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ListMarketingPromo)]
    [HttpPost("newsletter-subscribers")]
    public async Task<ApiResult<PagedResult<GetAllNewsletterSubscriberResponse>>> GetNewsletterSubscribers(
        GetAllNewsletterSubscriberRequest request)
        => await mediator.Send(request);
}
