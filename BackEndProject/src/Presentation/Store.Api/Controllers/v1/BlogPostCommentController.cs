using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.BlogPostComments.Queries;
using Edition.Application.Features.BlogPostComments.Commands;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Blog Post Comment
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-comment")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
public class BlogPostCommentController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchBlogPostCommentResponse>>> Search(SearchBlogPostCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateBlogPostCommentResponse>> Create(CreateBlogPostCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateBlogPostCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteBlogPostCommentRequest request)
        => await mediator.Send(request);
}