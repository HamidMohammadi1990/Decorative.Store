using Edition.Application.Features.ProductPropertyPrices.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductPropertyPrices;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductPropertyPriceMapperService : IMapper
{
    GetProductPropertyPriceResponse Map(ProductPropertyPrice model);
    GetAllProductPropertyPriceRequestDto Map(GetAllProductPropertyPriceRequest model);
    PagedResult<GetAllProductPropertyPriceResponse> Map(PagedResult<GetAllProductPropertyPriceDto> model);
}