using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Sections.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Sections
/// </summary>
[ApiVersion("1")]
[ControllerName("section")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class SectionController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchSectionResponse>>> Search(SearchSectionRequest request)
        => await mediator.Send(request);
}
