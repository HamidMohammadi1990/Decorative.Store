using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Dtos.SectionTypes;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class SectionTypeRepository
    (EditionDbContext context)
    : Repository<SectionType>(context), ISectionTypeRepository
{
    public async Task<PagedResult<SectionType>> GetAllAsync(GetAllSectionTypeRequestDto request)
    {
        var sectionTypes = await Context.SectionType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return sectionTypes;
    }

    public async Task<PagedResult<SectionType>> SearchAsync(SearchSectionTypeRequestDto request)
    {
        var sectionTypes = await Context.SectionType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return sectionTypes;
    }
}