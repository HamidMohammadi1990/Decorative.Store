using Edition.Application.Features.ProductPriceDeliveryOptions.Queries;
using Store.Domain.Dtos.ProductPriceDeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductPriceDeliveryOptionMapperService : IMapper
{
    GetProductPriceDeliveryOptionResponse Map(ProductPriceDeliveryOption model);
    GetAllProductPriceDeliveryOptionRequestDto Map(GetAllProductPriceDeliveryOptionRequest model);
    PagedResult<GetAllProductPriceDeliveryOptionResponse> Map(PagedResult<GetAllProductPriceDeliveryOptionResponseDto> model);
}