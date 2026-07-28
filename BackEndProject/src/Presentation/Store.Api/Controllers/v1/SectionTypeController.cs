using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.SectionTypes.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management SectionTypes
/// </summary>
[ApiVersion("1")]
[ControllerName("section-type")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class SectionTypeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchSectionTypeResponse>>> Search(SearchSectionTypeRequest request)
        => await mediator.Send(request);
}
