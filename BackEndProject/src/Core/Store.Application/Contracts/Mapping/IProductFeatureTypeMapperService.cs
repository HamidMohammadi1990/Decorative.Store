using Edition.Application.Features.ProductFeatureTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFeatureTypes;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductFeatureTypeMapperService : IMapper
{
    GetProductFeatureTypeResponse Map(ProductFeatureType model);
    GetAllProductFeatureTypeRequestDto Map(GetAllProductFeatureTypeRequest model);
    SearchProductFeatureTypeRequestDto Map(SearchProductFeatureTypeRequest model);
    PagedResult<GetAllProductFeatureTypeResponse> Map(PagedResult<ProductFeatureType> model);
    PagedResult<SearchProductFeatureTypeResponse> MapToSearch(PagedResult<ProductFeatureType> model);
}
