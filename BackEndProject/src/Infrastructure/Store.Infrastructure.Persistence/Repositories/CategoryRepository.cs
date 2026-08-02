using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.SubCategories;
using Edition.Application.Common.Extensions;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Categories;
using Store.Domain.Dtos.Localization;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CategoryRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Category>(context), ICategoryRepository
{
    public Task<Category?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.Category
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Category?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.Category
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(string code, int? excludeCategoryId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.Category.AnyAsync(
            x => x.Code == normalizedCode && (!excludeCategoryId.HasValue || x.Id != excludeCategoryId.Value),
            cancellationToken);
    }

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.CategoryTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludeCategoryId.HasValue || x.CategoryId != excludeCategoryId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllCategoryResponseDto>> GetAllAsync(
        GetAllCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var categories = Context.Category
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        categories = ApplyTranslationFilters(categories, request.Title, request.Slug);

        var result = await categories
            .Select(x => new GetAllCategoryResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive,
                Translations = x.Translations
                    .Select(t => new TranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug
                    })
                    .ToList()
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchCategoryResponseDto>> SearchAsync(
        SearchCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var categories = Context.Category
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        categories = ApplyTranslationFilters(categories, request.Title, request.Slug);

        var result = await categories
            .Select(x => new SearchCategoryResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Slug = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<CategoryWithSubCategoriesDto>> GetAllWithProductsAsync(
        ProductFeatureTypeCode featureType,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var rows = await
            (from product in Context.Product
             join productFeature in Context.ProductFeature on
                new { ProductId = product.Id, Value = featureType.ToValue() } equals
                new { productFeature.ProductId, productFeature.Value }
             join productFeatureType in Context.ProductFeatureType on
                new { productFeature.ProductFeatureTypeId, Type = featureType } equals
                new { ProductFeatureTypeId = productFeatureType.Id, productFeatureType.Type }
             join subCategory in Context.SubCategory on product.SubCategoryId equals subCategory.Id
             join category in Context.Category on subCategory.CategoryId equals category.Id
             select new
             {
                 product,
                 productTranslations = product.Translations,
                 subCategory,
                 category,
                 categoryTranslations = category.Translations,
                 subCategoryTranslations = subCategory.Translations
             })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(x => x.category.Id)
            .Select(group =>
            {
                var category = group.First().category;
                var categoryTranslation = ResolveTranslation(group.First().categoryTranslations, languageId, defaultLanguageId);

                return new CategoryWithSubCategoriesDto
                {
                    Id = category.Id,
                    Title = categoryTranslation.Title,
                    Slug = categoryTranslation.Slug,
                    SubCategories = group
                        .Select(x => new { x.subCategory, x.subCategoryTranslations })
                        .DistinctBy(x => x.subCategory.Id)
                        .Select(item =>
                        {
                            var subCategoryTranslation = ResolveTranslation(
                                item.subCategoryTranslations,
                                languageId,
                                defaultLanguageId);

                            return new SubCategoryWithProductsDto
                            {
                                Id = item.subCategory.Id,
                                Title = subCategoryTranslation.Title,
                                Slug = subCategoryTranslation.Slug,
                                Products = group
                                    .Where(x => x.subCategory.Id == item.subCategory.Id)
                                    .Select(x => new { x.product, x.productTranslations })
                                    .DistinctBy(x => x.product.Id)
                                    .Select(item => new ProductDto
                                    {
                                        Id = item.product.Id,
                                        Title = ProductTranslationHelper.ResolveTitle(
                                            item.productTranslations, languageId, defaultLanguageId),
                                        Slug = ProductTranslationHelper.ResolveSlug(
                                            item.productTranslations, languageId, defaultLanguageId)
                                    })
                                    .ToList()
                            };
                        })
                        .ToList()
                };
            })
            .ToList();
    }

    public async Task<List<CategoryWithSubCategoriesDto>> GetTreeAsync(CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var categories = await Context.Category
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Translations)
            .Include(x => x.SubCategories.Where(sub => sub.IsActive))
            .ThenInclude(sub => sub.Translations)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return categories
            .Select(category =>
            {
                var categoryTranslation = ResolveTranslation(category.Translations, languageId, defaultLanguageId);

                return new CategoryWithSubCategoriesDto
                {
                    Id = category.Id,
                    Title = categoryTranslation.Title,
                    Slug = categoryTranslation.Slug,
                    SubCategories = category.SubCategories
                        .OrderBy(sub => sub.Id)
                        .Select(subCategory =>
                        {
                            var subCategoryTranslation = ResolveTranslation(
                                subCategory.Translations,
                                languageId,
                                defaultLanguageId);

                            return new SubCategoryWithProductsDto
                            {
                                Id = subCategory.Id,
                                Title = subCategoryTranslation.Title,
                                Slug = subCategoryTranslation.Slug,
                                Products = []
                            };
                        })
                        .ToList()
                };
            })
            .ToList();
    }

    private static (string Title, string Slug) ResolveTranslation(
        IEnumerable<CategoryTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<CategoryTranslation> ?? translations.ToList();
        var current = list.FirstOrDefault(t => t.LanguageId == languageId);
        var fallback = list.FirstOrDefault(t => t.LanguageId == defaultLanguageId);

        return (
            current?.Title ?? fallback?.Title ?? string.Empty,
            current?.Slug ?? fallback?.Slug ?? string.Empty);
    }

    private static (string Title, string Slug) ResolveTranslation(
        IEnumerable<SubCategoryTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<SubCategoryTranslation> ?? translations.ToList();
        var current = list.FirstOrDefault(t => t.LanguageId == languageId);
        var fallback = list.FirstOrDefault(t => t.LanguageId == defaultLanguageId);

        return (
            current?.Title ?? fallback?.Title ?? string.Empty,
            current?.Slug ?? fallback?.Slug ?? string.Empty);
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }

    private static IQueryable<Category> ApplyTranslationFilters(IQueryable<Category> query, string? title, string? slug)
    {
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Translations.Any(t => t.Title.Contains(title)));

        if (!string.IsNullOrWhiteSpace(slug))
            query = query.Where(x => x.Translations.Any(t => t.Slug == slug));

        return query;
    }
}
