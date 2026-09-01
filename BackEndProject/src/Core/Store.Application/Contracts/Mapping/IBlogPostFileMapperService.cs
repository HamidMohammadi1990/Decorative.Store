using Edition.Application.Features.BlogPostFiles.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostFiles;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBlogPostFileMapperService : IMapper
{
    GetBlogPostFileResponse Map(BlogPostFile model, string title);
    GetAllBlogPostFileRequestDto Map(GetAllBlogPostFileRequest model);
    PagedResult<GetAllBlogPostFileResponse> Map(PagedResult<GetAllBlogPostFileResponseDto> model);
}
