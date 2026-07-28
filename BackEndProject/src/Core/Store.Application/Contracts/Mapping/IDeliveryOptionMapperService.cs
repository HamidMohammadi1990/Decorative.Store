using Edition.Application.Features.DeliveryOptions.Queries;
using Store.Domain.Dtos.DeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IDeliveryOptionMapperService : IMapper
{
    GetDeliveryOptionResponse Map(DeliveryOption model);
    GetAllDeliveryOptionRequestDto Map(GetAllDeliveryOptionRequest model);
    SearchDeliveryOptionRequestDto Map(SearchDeliveryOptionRequest model);
    PagedResult<GetAllDeliveryOptionResponse> Map(PagedResult<GetAllDeliveryOptionResponseDto> model);
    PagedResult<SearchDeliveryOptionResponse> Map(PagedResult<SearchDeliveryOptionResponseDto> model);
}