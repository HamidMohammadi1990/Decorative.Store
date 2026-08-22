using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostCategories.Queries;
using Store.Domain.Dtos.BlogPostCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBlogPostCategoryMapperService : IMapper
{
    GetBlogPostCategoryResponse Map(BlogPostCategory model, string title, string slug);
    PagedResult<GetAllBlogPostCategoryResponse> Map(PagedResult<GetAllBlogPostCategoryResponseDto> blogs);
    PagedResult<SearchBlogPostCategoryResponse> Map(PagedResult<SearchBlogPostCategoryResponseDto> model);
    GetAllBlogPostCategoryRequestDto Map(GetAllBlogPostCategoryRequest model);
    SearchBlogPostCategoryRequestDto Map(SearchBlogPostCategoryRequest model);
}
