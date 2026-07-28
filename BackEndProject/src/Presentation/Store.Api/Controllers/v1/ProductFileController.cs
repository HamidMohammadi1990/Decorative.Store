using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductFiles.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Files
/// </summary>
[ApiVersion("1")]
[ControllerName("product-file")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductFileController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductFileResponse>>> Search(SearchProductFileRequest request)
        => await mediator.Send(request);
}