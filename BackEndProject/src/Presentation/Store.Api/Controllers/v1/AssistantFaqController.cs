using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.AssistantFaqs.Queries;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Assistant FAQ items for storefront chat
/// </summary>
[ApiVersion("1")]
[ControllerName("assistant-faq")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class AssistantFaqStoreController(ISender mediator) : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchAssistantFaqResponse>>> Search(SearchAssistantFaqRequest request)
        => await mediator.Send(request);
}
