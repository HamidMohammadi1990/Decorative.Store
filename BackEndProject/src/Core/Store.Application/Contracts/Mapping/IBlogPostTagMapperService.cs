using Edition.Application.Features.BlogPostTags.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostTags;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBlogPostTagMapperService : IMapper
{
    GetBlogPostTagResponse Map(BlogPostTag model);
    SearchBlogPostTagRequestDto Map(SearchBlogPostTagRequest model);
    GetAllBlogPostTagRequestDto Map(GetAllBlogPostTagRequest model);
    PagedResult<SearchBlogPostTagResponse> Map(PagedResult<SearchBlogPostTagDto> model);
    PagedResult<GetAllBlogPostTagResponse> Map(PagedResult<GetAllBlogPostTagDto> blogPostTags);
}