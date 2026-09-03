using Edition.Application.Features.RoomTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RoomTypes;

namespace Edition.Application.Contracts.Mapping;

public interface IRoomTypeMapperService : IMapper
{
    GetAllRoomTypeRequestDto Map(GetAllRoomTypeRequest model);
    PagedResult<GetAllRoomTypeResponse> Map(PagedResult<GetAllRoomTypeResponseDto> model);
}