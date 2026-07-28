using Asp.Versioning;
using Edition.Application.Features.ProductPropertyRules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Property Rules
/// </summary>
[ApiVersion("1")]
[ControllerName("product-property-rule")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductPropertyRuleController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductPropertyRuleResponse>>> Search(SearchProductPropertyRuleRequest request)
        => await mediator.Send(request);
}
