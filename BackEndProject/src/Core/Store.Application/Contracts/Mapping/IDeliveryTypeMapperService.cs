using Edition.Application.Features.DeliveryTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.DeliveryTypes;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IDeliveryTypeMapperService : IMapper
{
    GetDeliveryTypeResponse Map(DeliveryType model);
    GetAllDeliveryTypeRequestDto Map(GetAllDeliveryTypeRequest model);
    SearchDeliveryTypeRequestDto Map(SearchDeliveryTypeRequest model);
    PagedResult<GetAllDeliveryTypeResponse> Map(PagedResult<GetAllDeliveryTypeResponseDto> model);
    PagedResult<SearchDeliveryTypeResponse> Map(PagedResult<SearchDeliveryTypeResponseDto> model);
}