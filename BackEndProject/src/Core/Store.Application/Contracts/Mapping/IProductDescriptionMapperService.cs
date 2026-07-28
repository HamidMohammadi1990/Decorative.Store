using Edition.Application.Features.ProductDescriptions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductDescriptions;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductDescriptionMapperService : IMapper
{
    GetProductDescriptionResponse Map(ProductDescription model);
    GetAllProductDescriptionRequestDto Map(GetAllProductDescriptionRequest model);
    SearchProductDescriptionRequestDto Map(SearchProductDescriptionRequest model);
    PagedResult<GetAllProductDescriptionResponse> Map(PagedResult<GetAllProductDescriptionResponseDto> model);
    PagedResult<SearchProductDescriptionResponse> Map(PagedResult<SearchProductDescriptionResponseDto> model);
}