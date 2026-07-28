using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Cities.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Cities
/// </summary>
[ApiVersion("1")]
[ControllerName("city")]
[ApiControllerCategory(ApiControllerCategory.Location)]
public class CityController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCityResponse>>> Search(SearchCityRequest request)
        => await mediator.Send(request);
}