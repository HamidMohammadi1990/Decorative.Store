using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Tags;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class TagRepository
    (EditionDbContext context)
    : Repository<Tag>(context), ITagRepository
{
    public async Task<PagedResult<Tag>> GetAllAsync(GetAllTagRequestDto request)
    {
        var tags = await Context.Tag
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return tags;
    }
}