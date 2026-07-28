using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Pages.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Pages
/// </summary>
[ApiVersion("1")]
[ControllerName("page")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class PageController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchPageResponse>>> Search(SearchPageRequest request)
        => await mediator.Send(request);

    [HttpPost("get-by-slug")]
    public async Task<ApiResult<GetPageBySlugResponse>> GetBySlug(GetPageBySlugRequest request)
        => await mediator.Send(request);
}
