using Edition.Application.Features.BlogPostComments.Queries;
using Store.Domain.Dtos.BlogPostComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBlogPostCommentMapperService : IMapper
{
    GetBlogPostCommentResponse Map(BlogPostComment model);
    GetAllBlogPostCommentRequestDto Map(GetAllBlogPostCommentRequest model);
    SearchBlogPostCommentRequestDto Map(SearchBlogPostCommentRequest model);
    PagedResult<GetAllBlogPostCommentResponse> Map(PagedResult<GetAllBlogPostCommentResponseDto> model);
    PagedResult<SearchBlogPostCommentResponse> Map(PagedResult<SearchBlogPostCommentResponseDto> model);
}