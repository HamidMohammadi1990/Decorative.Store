using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionTypes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISectionTypeRepository
{
    Task<PagedResult<GetAllSectionTypeResponseDto>> GetAllAsync(GetAllSectionTypeRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<SearchSectionTypeResponseDto>> SearchAsync(SearchSectionTypeRequestDto request, CancellationToken cancellationToken = default);
    void Add(SectionType model);
    void Remove(SectionType model);
    ValueTask<SectionType?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<SectionType?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<SectionType?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<SectionType?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<SectionType, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsTranslationAsync(
        int languageId,
        string name,
        int? excludeSectionTypeId = null,
        CancellationToken cancellationToken = default);
}
