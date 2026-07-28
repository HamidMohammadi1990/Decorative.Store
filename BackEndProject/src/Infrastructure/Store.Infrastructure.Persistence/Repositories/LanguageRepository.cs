using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Languages;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class LanguageRepository(EditionDbContext context)
    : Repository<Language>(context), ILanguageRepository
{
    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        => Context.Language.AnyAsync(cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<Language, bool>> expression, CancellationToken cancellationToken = default)
        => base.AnyAsync(expression, cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => Context.Language.CountAsync(cancellationToken);

    public async Task<IReadOnlyList<Language>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
        => await Context.Language
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<PagedResult<Language>> GetAllAsync(
        GetAllLanguageRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var languages = await Context.Language
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToPagedAsync(request.Pagination, cancellationToken);

        return languages;
    }

    public async Task<PagedResult<Language>> SearchAsync(
        SearchLanguageRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var languages = await Context.Language
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToPagedAsync(request.Pagination, cancellationToken);

        return languages;
    }

    public Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => Context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

    public Task<Language?> GetDefaultAsync(CancellationToken cancellationToken = default)
        => Context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);

    public void Add(Language language)
        => base.Add(language);

    public void AddRange(IEnumerable<Language> languages)
    {
        foreach (var language in languages)
            Context.Language.Add(language);
    }

    public void Remove(Language language)
        => base.Remove(language);

    public async Task ClearDefaultAsync(int exceptLanguageId, CancellationToken cancellationToken = default)
    {
        var languages = await Context.Language
            .Where(x => x.IsDefault && x.Id != exceptLanguageId)
            .ToListAsync(cancellationToken);

        foreach (var language in languages)
            language.ClearDefault();
    }
}
