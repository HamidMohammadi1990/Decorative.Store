using Edition.Application.Features.BlogPostLikes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostLikes;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBlogPostLikeMapperService : IMapper
{
    GetBlogPostLikeResponse Map(BlogPostLike model);
    GetAllBlogPostLikeRequestDto Map(GetAllBlogPostLikeRequest model);
    PagedResult<GetAllBlogPostLikeResponse> Map(PagedResult<BlogPostLikeDto> blogPostLikes);
}