using Edition.Application.Features.BlogPosts.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPosts;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBlogPostMapperService : IMapper
{
    GetBlogPostResponse Map(BlogPost model);
    GetAllBlogPostRequestDto Map(GetAllBlogPostRequest model);
    SearchBlogPostRequestDto Map(SearchBlogPostRequest model);
    PagedResult<GetAllBlogPostResponse> Map(PagedResult<GetAllBlogPostDto> model);
    PagedResult<SearchBlogPostResponse> Map(PagedResult<SearchBlogPostDto> model);
}