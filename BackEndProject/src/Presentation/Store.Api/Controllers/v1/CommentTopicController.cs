using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CommentTopics.Queries;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Comment topics for storefront
/// </summary>
[ApiVersion("1")]
[ControllerName("comment-topic")]
[ApiControllerCategory(ApiControllerCategory.Comments)]
public class CommentTopicController(ISender mediator) : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCommentTopicResponse>>> Search(SearchCommentTopicRequest request)
        => await mediator.Send(request);
}
