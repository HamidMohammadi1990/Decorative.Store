using Edition.Application.Features.Discounts.Queries;
using Store.Domain.Dtos.Discounts;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IDiscountMapperService : IMapper
{
    GetDiscountResponse Map(Discount model);
    GetAllDiscountRequestDto Map(GetAllDiscountRequest model);
    PagedResult<GetAllDiscountResponse> Map(PagedResult<GetAllDiscountResponseDto> model);
}