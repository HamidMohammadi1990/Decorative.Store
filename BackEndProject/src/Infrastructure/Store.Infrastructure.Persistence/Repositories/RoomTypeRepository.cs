using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RoomTypes;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class RoomTypeRepository
    (EditionDbContext context)
    : Repository<RoomType>(context), IRoomTypeRepository
{
    public Task<RoomType?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.RoomType
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(string code, int? excludeRoomTypeId = null, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim();
        return Context.RoomType.AnyAsync(
            x => x.Code == normalized && (!excludeRoomTypeId.HasValue || x.Id != excludeRoomTypeId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllRoomTypeResponseDto>> GetAllAsync(
        GetAllRoomTypeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var query = Context.RoomType
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Code))
            query = query.Where(x => x.Code.Contains(request.Code.Trim()));

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (request.LanguageId.HasValue)
        {
            var languageId = request.LanguageId.Value;
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var title = request.Title.Trim();
                query = query.Where(x => x.Translations.Any(t =>
                    t.LanguageId == languageId && t.Title.Contains(title)));
            }

            return await query
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.Id)
                .Select(x => new GetAllRoomTypeResponseDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    ImageFileName = x.ImageFileName,
                    Priority = x.Priority,
                    IsActive = x.IsActive,
                    Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault(),
                    LanguageId = languageId,
                })
                .ToPagedAsync(request.Pagination);
        }

        return await query
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .Select(x => new GetAllRoomTypeResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                ImageFileName = x.ImageFileName,
                Priority = x.Priority,
                IsActive = x.IsActive,
                Title = x.Translations.Select(t => t.Title).FirstOrDefault(),
                LanguageId = x.Translations.Select(t => (int?)t.LanguageId).FirstOrDefault(),
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<IReadOnlyList<ListRoomTypeResponseDto>> GetActiveForLanguageAsync(
        int languageId,
        CancellationToken cancellationToken = default)
    {
        return await Context.RoomType
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Where(x => x.Translations.Any(t => t.LanguageId == languageId))
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .Select(x => new ListRoomTypeResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                Title = x.Translations.First(t => t.LanguageId == languageId).Title,
                ImageFileName = x.ImageFileName,
                Priority = x.Priority,
            })
            .ToListAsync(cancellationToken);
    }
}
