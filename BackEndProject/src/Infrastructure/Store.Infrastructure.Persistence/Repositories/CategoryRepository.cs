using System.Linq.Expressions;
using Edition.Domain.Dtos.Products;
using Microsoft.EntityFrameworkCore;
using Edition.Domain.Dtos.SubCategories;
using Edition.Application.Common.Extensions;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Categories;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CategoryRepository
    (EditionDbContext context)
    : Repository<Category>(context), ICategoryRepository
{
    public async Task<PagedResult<GetAllCategoryResponseDto>> GetAllAsync(GetAllCategoryRequestDto request)
    {
        var categories = Context.Category            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await categories
            .AsNoTracking()
            .Select(x => new GetAllCategoryResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchCategoryResponseDto>> SearchAsync(SearchCategoryRequestDto request)
    {
        var categories = Context
            .Category            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        var result =
            await categories
            .AsNoTracking()
            .Select(x => new SearchCategoryResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<CategoryWithSubCategoriesDto>> GetAllWithProductsAsync(ProductFeatureTypeCode featureType)
    {
        var categories = await
            (from product in Context.Product
             join productFeature in Context.ProductFeature on
                new { ProductId = product.Id, Value = featureType.ToValue() } equals
                new { productFeature.ProductId, productFeature.Value }
             join productFeatureType in Context.ProductFeatureType on
                new { productFeature.ProductFeatureTypeId, Type = featureType } equals
                new { ProductFeatureTypeId = productFeatureType.Id, productFeatureType.Type }
             join subCategory in Context.SubCategory on product.SubCategoryId equals subCategory.Id
             join category in Context.Category on subCategory.CategoryId equals category.Id
             group new { product, subCategory } by new { category.Id, category.Title, category.Slug } into groups
             select new CategoryWithSubCategoriesDto
             {
                 Id = groups.Key.Id,
                 Title = groups.Key.Title,
                 Slug = groups.Key.Slug,
                 SubCategories = (
                    from subCategory in groups.Select(x => x.subCategory).Distinct()
                    select new SubCategoryWithProductsDto
                    {
                        Id = subCategory.Id,
                        Title = subCategory.Title,
                        Slug = subCategory.Slug,
                        Products = (
                            from product in groups.Where(x => x.subCategory.Id == subCategory.Id).Select(x => x.product).Distinct()
                            select new ProductDto
                            {
                                Id = product.Id,
                                Title = product.Title,
                                Slug = product.Slug
                            }
                        ).ToList()
                    }
                ).ToList()
             })
             .AsNoTracking()
             .ToListAsync();

        return categories;
    }
}