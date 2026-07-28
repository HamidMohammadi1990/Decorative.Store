using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CompanyStories.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Company Story
/// </summary>
[ApiVersion("1")]
[ControllerName("company-story")]
[ApiControllerCategory(ApiControllerCategory.Company)]
public class CompanyStoryController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCompanyStoryResponse>>> Search(SearchCompanyStoryRequest request)
        => await mediator.Send(request);
}
