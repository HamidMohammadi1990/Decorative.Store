using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PostTypes.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Post Types
/// </summary>
[ApiVersion("1")]
[ControllerName("post-type")]
[ApiControllerCategory(ApiControllerCategory.PostType)]
public class PostTypeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchPostTypeResponse>>> Search(SearchPostTypeRequest request)
        => await mediator.Send(request);
}