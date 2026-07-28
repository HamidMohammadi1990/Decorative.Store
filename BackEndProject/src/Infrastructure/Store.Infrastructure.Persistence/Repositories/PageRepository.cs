using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Pages;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PageRepository
    (EditionDbContext context)
    : Repository<Page>(context), IPageRepository
{
    public async Task<PagedResult<Page>> GetAllAsync(GetAllPageRequestDto request)
    {
        var pages = Context.Page            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await pages
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<Page>> SearchAsync(SearchPageRequestDto request)
    {
        var pages = Context.Page            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        return await pages
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<Page?> GetActiveBySlugWithContentAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await Context.Page
            .AsNoTracking()
            .Include(p => p.PageSections)
                .ThenInclude(ps => ps.Section)
                    .ThenInclude(s => s.SectionType)
            .Include(p => p.PageSections)
                .ThenInclude(ps => ps.Section)
                    .ThenInclude(s => s.SectionItems)
            .FirstOrDefaultAsync(p => p.Slug == slug.Trim() && p.IsActive, cancellationToken);
    }

    public Task<bool> HasPageSectionsAsync(int pageId, CancellationToken cancellationToken = default)
        => Context.PageSection.AnyAsync(x => x.PageId == pageId, cancellationToken);
}