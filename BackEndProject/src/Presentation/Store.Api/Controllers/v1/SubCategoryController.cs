using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.SubCategories.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Sub Categories
/// </summary>
[ApiVersion("1")]
[ControllerName("sub-category")]
[ApiControllerCategory(ApiControllerCategory.Catalog)]
public class SubCategoryController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchSubCategoryResponse>>> Search(SearchSubCategoryRequest request)
        => await mediator.Send(request);
}