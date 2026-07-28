using Edition.Application.Features.ProductComments.Queries;
using Store.Domain.Dtos.ProductComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductCommentMapperService : IMapper
{
    GetProductCommentResponse Map(ProductComment model);
    GetAllProductCommentRequestDto Map(GetAllProductCommentRequest model);
    SearchProductCommentRequestDto Map(SearchProductCommentRequest model);
    PagedResult<GetAllProductCommentResponse> Map(PagedResult<GetAllProductCommentResponseDto> model);
    PagedResult<SearchProductCommentResponse> Map(PagedResult<SearchProductCommentResponseDto> model);
}