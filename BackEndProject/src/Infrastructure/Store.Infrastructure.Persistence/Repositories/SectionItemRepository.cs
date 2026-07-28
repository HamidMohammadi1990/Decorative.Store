using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.SectionItems;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class SectionItemRepository
    (EditionDbContext context)
    : Repository<SectionItem>(context), ISectionItemRepository
{
    public async Task<PagedResult<SectionItem>> GetAllAsync(GetAllSectionItemRequestDto request)
    {
        var sectionItems = Context.SectionItem
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await sectionItems
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SectionItem>> SearchAsync(SearchSectionItemRequestDto request)
    {
        var sectionItems = await Context.SectionItem
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return sectionItems;
    }
}