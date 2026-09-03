using Asp.Versioning;
using Edition.Application.Features.Seo.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Attributes;
using Store.Common.Enums;
using Store.WebFramework.Api;

namespace Store.Api.Controllers.v1;

[ApiVersion("1")]
[ControllerName("seo")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class SeoStoreController(ISender mediator) : BaseApiController
{
    [HttpGet("sitemap.xml")]
    [Produces("application/xml")]
    public async Task<IActionResult> Sitemap(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSitemapRequest(), cancellationToken);
        if (!result.IsSuccess || result.Result is null)
            return NotFound();

        return Content(result.Result, "application/xml; charset=utf-8");
    }
}
