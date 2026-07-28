using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Sections;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class SectionRepository
    (EditionDbContext context)
    : Repository<Section>(context), ISectionRepository
{
    public async Task<PagedResult<Section>> GetAllAsync(GetAllSectionRequestDto request)
    {
        var sections = await Context.Section
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return sections;
    }

    public async Task<PagedResult<Section>> SearchAsync(SearchSectionRequestDto request)
    {
        var now = DateTime.UtcNow;
        var sections = await Context.Section
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .Where(x => x.SectionType.IsActive)
            .Where(x => !x.StartDateOnUtc.HasValue || x.StartDateOnUtc <= now)
            .Where(x => !x.EndDateOnUtc.HasValue || x.EndDateOnUtc >= now)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return sections;
    }
}