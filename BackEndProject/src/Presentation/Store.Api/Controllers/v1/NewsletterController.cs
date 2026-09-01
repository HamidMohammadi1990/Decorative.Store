using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.NewsletterSubscribers.Commands;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Newsletter subscription for storefront footer
/// </summary>
[ApiVersion("1")]
[ControllerName("newsletter")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class NewsletterController(ISender mediator) : BaseApiController
{
    [HttpPost("subscribe")]
    public async Task<ApiResult<OperationResult>> Subscribe(SubscribeNewsletterRequest request)
        => await mediator.Send(request);
}
