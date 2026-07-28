using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.SectionItems.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management SectionItems
/// </summary>
[ApiVersion("1")]
[ControllerName("section-item")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class SectionItemController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchSectionItemResponse>>> Search(SearchSectionItemRequest request)
        => await mediator.Send(request);
}
