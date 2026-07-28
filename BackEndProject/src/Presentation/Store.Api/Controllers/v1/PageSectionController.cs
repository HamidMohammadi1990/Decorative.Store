using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PageSections.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management PageSections
/// </summary>
[ApiVersion("1")]
[ControllerName("page-section")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class PageSectionController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchPageSectionResponse>>> Search(SearchPageSectionRequest request)
        => await mediator.Send(request);
}
