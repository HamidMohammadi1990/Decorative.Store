using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PageSections;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PageSectionRepository
    (EditionDbContext context)
    : Repository<PageSection>(context), IPageSectionRepository
{
    public async Task<PagedResult<PageSection>> GetAllAsync(GetAllPageSectionRequestDto request)
    {
        var pageSections = Context.PageSection
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await pageSections
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<PageSection>> SearchAsync(SearchPageSectionRequestDto request)
    {
        var pageSections = Context.PageSection
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await pageSections
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }
}