using Store.Common.Models;

namespace Edition.Application.Features.BlogPosts.Queries;

public record GetBlogPostDetailRequest(string Slug)
    : IRequest<OperationResult<GetBlogPostDetailResponse>>;
