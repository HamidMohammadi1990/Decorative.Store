using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductDescriptions.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Descriptions
/// </summary>
[ApiVersion("1")]
[ControllerName("product-description")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductDescriptionController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductDescriptionResponse>>> Search(SearchProductDescriptionRequest request)
        => await mediator.Send(request);
}