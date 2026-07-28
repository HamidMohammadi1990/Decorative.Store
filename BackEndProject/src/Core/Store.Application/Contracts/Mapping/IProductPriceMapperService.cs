using Edition.Application.Features.ProductPrices.Queries;
using Store.Domain.Dtos.ProductPrices;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductPriceMapperService : IMapper
{
    GetProductPriceResponse Map(ProductPrice model);
    GetAllProductPriceRequestDto Map(GetAllProductPriceRequest model);
    PagedResult<GetAllProductPriceResponse> Map(PagedResult<GetAllProductPriceDto> model);
}