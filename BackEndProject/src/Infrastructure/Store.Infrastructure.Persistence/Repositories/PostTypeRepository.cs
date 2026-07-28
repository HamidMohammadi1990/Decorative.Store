using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.PostTypes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PostTypeRepository
     (EditionDbContext context)
    : Repository<PostType>(context), IPostTypeRepository
{
    public async Task<PagedResult<GetAllPostTypeResponseDto>> GetAllAsync(GetAllPostTypeRequestDto request)
    {
        var postTypes = Context.PostType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await postTypes
            .Select(x => new GetAllPostTypeResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                IsActive = x.IsActive,
                Description = x.Description
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchPostTypeResponseDto>> SearchAsync(SearchPostTypeRequestDto request)
    {
        var postTypes = Context.PostType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        var result =
            await postTypes
            .Select(x => new SearchPostTypeResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                Description = x.Description
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<PostType>> GetAllAsync()
    {
        var postTypes =
            await Context
            .PostType
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToListAsync();

        return postTypes;
    }
}