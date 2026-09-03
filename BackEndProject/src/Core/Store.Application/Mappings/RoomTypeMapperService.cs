using Edition.Application.Common.Directories;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.RoomTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RoomTypes;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class RoomTypeMapperService : IRoomTypeMapperService
{
    public GetAllRoomTypeRequestDto Map(GetAllRoomTypeRequest model)
        => new GetAllRoomTypeRequestDto
        {
            LanguageId = model.LanguageId,
            Title = model.Title,
            Code = model.Code,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<RoomType, GetAllRoomTypeRequestDto>(model);

    public PagedResult<GetAllRoomTypeResponse> Map(PagedResult<GetAllRoomTypeResponseDto> model)
    {
        var items = model.Items
            .Select(x => new GetAllRoomTypeResponse
            {
                Id = x.Id,
                Code = x.Code,
                ImageFileName = x.ImageFileName,
                ImageUrl = RoomTypeDirectory.GetImageUrl(x.ImageFileName),
                Priority = x.Priority,
                IsActive = x.IsActive,
                Title = x.Title,
                LanguageId = x.LanguageId,
            })
            .ToList();

        return PagedResult<GetAllRoomTypeResponse>.Create(items, model);
    }
}
