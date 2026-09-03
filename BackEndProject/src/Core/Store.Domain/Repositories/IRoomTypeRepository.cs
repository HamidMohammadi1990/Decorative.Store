using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RoomTypes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IRoomTypeRepository
{
    void Add(RoomType roomType);

    ValueTask<RoomType?> FindAsync(int id, CancellationToken cancellationToken);

    void Remove(RoomType roomType);

    Task<RoomType?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsCodeAsync(string code, int? excludeRoomTypeId = null, CancellationToken cancellationToken = default);

    Task<PagedResult<GetAllRoomTypeResponseDto>> GetAllAsync(
        GetAllRoomTypeRequestDto request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ListRoomTypeResponseDto>> GetActiveForLanguageAsync(
        int languageId,
        CancellationToken cancellationToken = default);
}
