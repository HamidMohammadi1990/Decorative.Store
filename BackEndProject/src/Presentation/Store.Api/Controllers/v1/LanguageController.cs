using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Languages.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Public languages lookup.
/// </summary>
[ApiVersion("1")]
[ControllerName("language")]
[ApiControllerCategory(ApiControllerCategory.Localization)]
public class LanguageController(ISender mediator) : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchLanguageResponse>>> Search(SearchLanguageRequest request)
        => await mediator.Send(request);
}