using Edition.Application.Features.ProductProperties.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductProperties;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductPropertyMapperService : IMapper
{
    GetProductPropertyResponse Map(ProductProperty model);
    GetAllProductPropertyRequestDto Map(GetAllProductPropertyRequest model);
    SearchProductPropertyRequestDto Map(SearchProductPropertyRequest model);
    PagedResult<GetAllProductPropertyResponse> Map(PagedResult<ProductProperty> model);
    PagedResult<SearchProductPropertyResponse> MapToSearch(PagedResult<ProductProperty> model);
}
