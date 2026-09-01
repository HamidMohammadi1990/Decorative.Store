using Edition.Application.Features.MarketingPromos.Queries;
using Store.Domain.Dtos.MarketingPromos;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IMarketingPromoMapperService : IMapper
{
    GetAllMarketingPromoRequestDto Map(GetAllMarketingPromoRequest model);
    PagedResult<GetAllMarketingPromoResponse> Map(PagedResult<GetAllMarketingPromoResponseDto> model);
    GetMarketingPromoResponse Map(MarketingPromo model);
}
