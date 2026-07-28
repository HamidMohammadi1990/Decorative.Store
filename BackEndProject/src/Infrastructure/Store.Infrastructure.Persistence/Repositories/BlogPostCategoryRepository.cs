using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.BlogPostCategories;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostCategoryRepository
    (EditionDbContext context)
    : Repository<BlogPostCategory>(context), IBlogPostCategoryRepository
{
    public async Task<PagedResult<GetAllBlogPostCategoryResponseDto>> GetAllAsync(GetAllBlogPostCategoryRequestDto request)
    {
        var categories = Context.BlogPostCategory
           .ApplyContentPolicyFilter(request.ContentFilter)
           .ApplyQueryFilters(request);

        var result = await
            categories
            .Select(x => new GetAllBlogPostCategoryResponseDto
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchBlogPostCategoryResponseDto>> SearchAsync(SearchBlogPostCategoryRequestDto request)
    {
        var categories = Context.BlogPostCategory
           .ApplyContentPolicyFilter(request.ContentFilter)
           .Where(x => x.IsActive)
           .ApplyQueryFilters(request);

        var result = await
            categories
            .Select(x => new SearchBlogPostCategoryResponseDto
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}