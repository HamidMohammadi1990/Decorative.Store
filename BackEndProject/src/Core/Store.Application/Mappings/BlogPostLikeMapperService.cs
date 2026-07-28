using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostLikes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostLikes;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BlogPostLikeMapperService : IBlogPostLikeMapperService
{
    public PagedResult<GetAllBlogPostLikeResponse> Map(PagedResult<BlogPostLikeDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBlogPostLikeResponse
            {
                Id = x.Id,
                UserName = x.UserName,
                ClientIP = x.ClientIP,
                BlogPostId = x.BlogPostId,
                CreatedOnUtc = x.CreatedOnUtc,
                BlogPostTitle = x.BlogPostTitle
            })
            .ToList();

        return PagedResult<GetAllBlogPostLikeResponse>.Create(items, model);
    }

    public GetAllBlogPostLikeRequestDto Map(GetAllBlogPostLikeRequest model)
    {
        return new GetAllBlogPostLikeRequestDto
        {
            BlogPostId = model.BlogPostId,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPostLike, GetAllBlogPostLikeRequestDto>(model);
    }

    public GetBlogPostLikeResponse Map(BlogPostLike model)
    {
        return new GetBlogPostLikeResponse
        {
            Id = model.BlogPostId,
            UserId = model.UserId,
            ClientIP = model.ClientIP,
            BlogPostId = model.BlogPostId,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }
}