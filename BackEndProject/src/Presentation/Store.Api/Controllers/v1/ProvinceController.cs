using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Provinces.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Provinces
/// </summary>
[ApiVersion("1")]
[ControllerName("province")]
[ApiControllerCategory(ApiControllerCategory.Location)]
public class ProvinceController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProvinceResponse>>> Search(SearchProvinceRequest request)
        => await mediator.Send(request);
}