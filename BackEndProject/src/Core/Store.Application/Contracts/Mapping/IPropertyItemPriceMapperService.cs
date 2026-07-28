using Edition.Application.Features.PropertyItemPrices.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItemPrices;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPropertyItemPriceMapperService : IMapper
{
    GetPropertyItemPriceResponse Map(PropertyItemPrice model);
    GetAllPropertyItemPriceRequestDto Map(GetAllPropertyItemPriceRequest model);
    PagedResult<GetAllPropertyItemPriceResponse> Map(PagedResult<GetAllPropertyItemPriceDto> model);
}