using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.SubCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class SubCategoryRepository
    (EditionDbContext context)
    : Repository<SubCategory>(context), ISubCategoryRepository
{
    public async Task<PagedResult<GetAllSubCategoryResponseDto>> GetAllAsync(GetAllSubCategoryRequestDto request)
    {
        var subCategorySource = Context.SubCategory
            .ApplyContentPolicyFilter(request.ContentFilter);

        var categories =
            from subCategory in subCategorySource
            join category in Context.Category on subCategory.CategoryId equals category.Id
            select new { category, subCategory };

        categories = categories.ApplyQueryFilters(request);

        var result =
            await categories
            .Select(x => new GetAllSubCategoryResponseDto
            {
                Id = x.subCategory.Id,
                Code = x.subCategory.Code,
                Slug = x.subCategory.Slug,
                Title = x.subCategory.Title,
                IsActive = x.subCategory.IsActive,
                CategoryId = x.subCategory.CategoryId,
                CategoryTitle = x.category.Title,
                CategoryCode = x.category.Code,
                CategorySlug = x.category.Slug
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchSubCategoryResponseDto>> SearchAsync(SearchSubCategoryRequestDto request)
    {
        var subCategorySource = Context.SubCategory
            .ApplyContentPolicyFilter(request.ContentFilter);

        var categories =
            from subCategory in subCategorySource
            join category in Context.Category on subCategory.CategoryId equals category.Id
            where subCategory.IsActive
            select new { category, subCategory };

        categories = categories.ApplyQueryFilters(request);

        var result =
            await categories
            .Select(x => new SearchSubCategoryResponseDto
            {
                Id = x.subCategory.Id,
                Code = x.subCategory.Code,
                Slug = x.subCategory.Slug,
                Title = x.subCategory.Title,
                CategoryId = x.subCategory.CategoryId,
                CategoryTitle = x.category.Title,
                CategoryCode = x.category.Code,
                CategorySlug = x.category.Slug
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}