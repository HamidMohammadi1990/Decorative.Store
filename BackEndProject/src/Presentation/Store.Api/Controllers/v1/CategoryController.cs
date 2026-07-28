using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Categories.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Categories
/// </summary>
[ApiVersion("1")]
[ControllerName("category")]
[ApiControllerCategory(ApiControllerCategory.Catalog)]
public class CategoryController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCategoryResponse>>> Search(SearchCategoryRequest request)
        => await mediator.Send(request);

    [HttpGet("tree")]
    public async Task<ApiResult<List<GetCategoriesWithProductsResponse>>> GetTree()
        => await mediator.Send(new GetCategoriesWithProductsRequest());
}