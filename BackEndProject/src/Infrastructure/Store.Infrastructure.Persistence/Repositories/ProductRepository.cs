using System.Linq.Expressions;
using Edition.Application.Contracts.Localization;
using Edition.Application.Features.Catalog.Services;
using Microsoft.EntityFrameworkCore;
using Store.Common.Catalog;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.Catalog;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Product>(context), IProductRepository
{
    public Task<Product?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.Product
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Product?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.Product
            .AsNoTracking()
            .Include(x => x.Translations)
            .Include(x => x.SubCategory)
                .ThenInclude(sc => sc.Translations)
            .Include(x => x.SubCategory)
                .ThenInclude(sc => sc.Category)
                .ThenInclude(c => c.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsProductCodeAsync(string productCode, int? excludeProductId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = productCode.Trim();
        return Context.Product.AnyAsync(
            x => x.ProductCode == normalizedCode && (!excludeProductId.HasValue || x.Id != excludeProductId.Value),
            cancellationToken);
    }

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeProductId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.ProductTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludeProductId.HasValue || x.ProductId != excludeProductId.Value),
            cancellationToken);
    }

    public async Task<ProductSummaryDto?> GetProductSummaryByIdAsync(int id)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        return await Context
            .Product
            .Include(x => x.ProductFiles)
                .ThenInclude(f => f.Translations)
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ProductSummaryDto
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                Slug = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Description = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Description)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Description)
                        .FirstOrDefault()
                    ?? string.Empty,
                SubCategoryTitle = x.SubCategory!.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.SubCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Images = x.ProductFiles!
                    .OrderByDescending(file => file.IsMain)
                    .ThenBy(file => file.Id)
                    .Select(file => new CheckoutProductImageDto
                    {
                        Url = file.FileName,
                        Title = file.Translations
                                .Where(t => t.LanguageId == languageId)
                                .Select(t => t.Title)
                                .FirstOrDefault()
                            ?? file.Translations
                                .Where(t => t.LanguageId == defaultLanguageId)
                                .Select(t => t.Title)
                                .FirstOrDefault()
                            ?? string.Empty
                    }).ToList()
            })
            .SingleOrDefaultAsync();
    }

    public async Task<PagedResult<GetAllProductResponseDto>> GetAllAsync(
        GetAllProductRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var productSource = Context.Product
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from product in productSource
            join subCategory in Context.SubCategory on product.SubCategoryId equals subCategory.Id
            join category in Context.Category on subCategory.CategoryId equals category.Id
            select new { product, subCategory, category };

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.category.Id == request.CategoryId.Value);

        if (request.SubCategoryId.HasValue)
            query = query.Where(x => x.product.SubCategoryId == request.SubCategoryId.Value);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.product.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.ProductCode))
            query = query.Where(x => x.product.ProductCode.Contains(request.ProductCode));

        if (!string.IsNullOrWhiteSpace(request.Title))
            query = query.Where(x => x.product.Translations.Any(t => t.Title.Contains(request.Title)));

        if (!string.IsNullOrWhiteSpace(request.Slug))
            query = query.Where(x => x.product.Translations.Any(t => t.Slug == request.Slug));

        if (!string.IsNullOrWhiteSpace(request.CategorySlug))
            query = query.Where(x => x.category.Translations.Any(t => t.Slug == request.CategorySlug));

        if (!string.IsNullOrWhiteSpace(request.SubCategorySlug))
            query = query.Where(x => x.subCategory.Translations.Any(t => t.Slug == request.SubCategorySlug));

        var result = await query
            .Select(x => new GetAllProductResponseDto
            {
                Id = x.product.Id,
                ProductCode = x.product.ProductCode,
                IsActive = x.product.IsActive,
                CreatedOnUtc = x.product.CreatedOnUtc,
                SubCategoryId = x.product.SubCategoryId,
                Translations = x.product.Translations
                    .Select(t => new ProductTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug,
                        Description = t.Description
                    })
                    .ToList()
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<CatalogListingDto> GetCatalogListingByPathAsync(
        string catalogPath,
        CancellationToken cancellationToken = default)
    {
        var normalizedPath = CatalogSlugNormalizer.NormalizePath(catalogPath);
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(normalizedPath))
        {
            return new CatalogListingDto
            {
                PathNotFound = true,
                Breadcrumbs =
                [
                    new CatalogBreadcrumbDto(
                        await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken),
                        "/")
                ]
            };
        }

        var (collectionSlug, categoryPath) = ParseCollectionPrefix(normalizedPath);
        if (collectionSlug is not null)
        {
            return await WithEnrichedProductsAsync(
                await BuildCollectionListingAsync(
                    collectionSlug,
                    categoryPath,
                    languageId,
                    defaultLanguageId,
                    cancellationToken),
                languageId,
                defaultLanguageId,
                cancellationToken);
        }

        var subCategoryMatch = await FindActiveSubCategoryByNormalizedSlugAsync(
            normalizedPath,
            categoryId: null,
            languageId,
            defaultLanguageId,
            cancellationToken);

        if (subCategoryMatch is not null)
        {
            return await WithEnrichedProductsAsync(
                await BuildSubCategoryListingAsync(
                    subCategoryMatch.Value.SubCategoryId,
                    subCategoryMatch.Value.CategoryId,
                    languageId,
                    defaultLanguageId,
                    cancellationToken),
                languageId,
                defaultLanguageId,
                cancellationToken);
        }

        if (TryParseSplitCategoryPath(normalizedPath, out var categorySegment, out var subCategorySegment))
        {
            var splitMatch = await TryMatchCategorySubCategorySegmentsAsync(
                categorySegment,
                subCategorySegment,
                languageId,
                defaultLanguageId,
                cancellationToken);

            if (splitMatch is not null)
            {
                return await WithEnrichedProductsAsync(
                    await BuildSubCategoryListingAsync(
                        splitMatch.Value.SubCategoryId,
                        splitMatch.Value.CategoryId,
                        languageId,
                        defaultLanguageId,
                        cancellationToken),
                    languageId,
                    defaultLanguageId,
                    cancellationToken);
            }
        }

        var categoryMatch = await FindActiveCategoryIdByNormalizedSlugAsync(
            normalizedPath,
            languageId,
            defaultLanguageId,
            cancellationToken);

        if (categoryMatch != default)
        {
            var categoryTranslation = await ResolveCategoryTranslationAsync(
                categoryMatch,
                languageId,
                defaultLanguageId,
                cancellationToken);

            var subCategoryIds = await Context.SubCategory
                .AsNoTracking()
                .Where(x => x.IsActive && x.CategoryId == categoryMatch)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
            var breadcrumbs = new List<CatalogBreadcrumbDto>
            {
                new(homeLabel, "/"),
                new(categoryTranslation.Title, $"/{categoryTranslation.Slug}")
            };

            var products = await LoadCatalogProductsAsync(
                subCategoryIds,
                languageId,
                defaultLanguageId,
                cancellationToken);

            return await WithEnrichedProductsAsync(
                new CatalogListingDto
                {
                    Title = categoryTranslation.Title,
                    Breadcrumbs = breadcrumbs,
                    Products = products
                },
                languageId,
                defaultLanguageId,
                cancellationToken);
        }

        if (normalizedPath == "in-stock")
        {
            var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
            var title = await ResolveCollectionLabelAsync("in-stock", languageId, defaultLanguageId, cancellationToken);
            var products = await GetInStockCatalogProductsAsync(cancellationToken: cancellationToken);

            return await WithEnrichedProductsAsync(
                new CatalogListingDto
                {
                    Title = title,
                    Breadcrumbs =
                    [
                        new(homeLabel, "/"),
                        new(title, "/in-stock")
                    ],
                    Products = products
                },
                languageId,
                defaultLanguageId,
                cancellationToken);
        }

        if (normalizedPath == "best-sellers")
        {
            var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
            var title = await ResolveCollectionLabelAsync("best-sellers", languageId, defaultLanguageId, cancellationToken);
            var products = await GetBestSellingCatalogProductsAsync(cancellationToken: cancellationToken);

            return await WithEnrichedProductsAsync(
                new CatalogListingDto
                {
                    Title = title,
                    Breadcrumbs =
                    [
                        new(homeLabel, "/"),
                        new(title, "/best-sellers")
                    ],
                    Products = products
                },
                languageId,
                defaultLanguageId,
                cancellationToken);
        }

        return new CatalogListingDto
        {
            PathNotFound = true,
            Title = normalizedPath,
            Breadcrumbs =
            [
                new CatalogBreadcrumbDto(
                    await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken),
                    "/")
            ]
        };
    }

    public async Task<CatalogSearchDto> SearchCatalogAsync(
        string query,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var term = query.Trim();
        if (string.IsNullOrWhiteSpace(term))
            return new CatalogSearchDto();

        var take = limit > 0 ? limit : 8;
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var categoryRows = await Context.Category
            .AsNoTracking()
            .Where(category => category.IsActive)
            .Include(category => category.Translations)
            .Where(category => category.Translations.Any(t => t.Title.Contains(term) || t.Slug.Contains(term)))
            .OrderBy(category => category.Id)
            .Take(take)
            .ToListAsync(cancellationToken);

        var categories = categoryRows
            .Select(category => new CatalogSearchCategoryDto
            {
                Title = ResolveCategoryTranslationValue(
                    category.Translations, languageId, defaultLanguageId, translation => translation.Title),
                Slug = ResolveCategoryTranslationValue(
                    category.Translations, languageId, defaultLanguageId, translation => translation.Slug)
            })
            .Where(category => !string.IsNullOrWhiteSpace(category.Slug))
            .ToList();

        var subCategoryRows = await Context.SubCategory
            .AsNoTracking()
            .Where(subCategory => subCategory.IsActive && subCategory.Category.IsActive)
            .Include(subCategory => subCategory.Translations)
            .Include(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .Where(subCategory =>
                subCategory.Translations.Any(t => t.Title.Contains(term) || t.Slug.Contains(term)))
            .OrderBy(subCategory => subCategory.Id)
            .Take(take)
            .ToListAsync(cancellationToken);

        var subCategories = subCategoryRows
            .Select(subCategory => new CatalogSearchSubCategoryDto
            {
                Title = ResolveSubCategoryTranslationValue(
                    subCategory.Translations, languageId, defaultLanguageId, translation => translation.Title),
                Slug = ResolveSubCategoryTranslationValue(
                    subCategory.Translations, languageId, defaultLanguageId, translation => translation.Slug),
                CategoryTitle = ResolveCategoryTranslationValue(
                    subCategory.Category.Translations, languageId, defaultLanguageId, translation => translation.Title),
                CategorySlug = ResolveCategoryTranslationValue(
                    subCategory.Category.Translations, languageId, defaultLanguageId, translation => translation.Slug)
            })
            .Where(subCategory => !string.IsNullOrWhiteSpace(subCategory.Slug))
            .ToList();

        var products = await LoadFilteredCatalogProductsAsync(
            product => product.Translations.Any(t => t.Title.Contains(term) || t.Slug.Contains(term)),
            take,
            cancellationToken);

        return new CatalogSearchDto
        {
            Categories = categories,
            SubCategories = subCategories,
            Products = products
        };
    }

    public async Task<CatalogProductDto?> GetCatalogProductBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedSlug))
            return new CatalogProductDto { NotFound = true };

        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var product = await Context.Product
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Translations)
            .Include(x => x.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(x => x.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(x => x.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .FirstOrDefaultAsync(
                x => x.Translations.Any(t => t.Slug.ToLower() == normalizedSlug),
                cancellationToken);

        if (product is null)
            return new CatalogProductDto { NotFound = true };

        var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
        var productSlug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
        var description = ResolveProductTranslation(
            product.Translations,
            languageId,
            defaultLanguageId,
            t => t.Description);

        var mainFile = product.ProductFiles
            .OrderByDescending(file => file.IsMain)
            .ThenBy(file => file.Id)
            .FirstOrDefault();
        var imageAlt = mainFile is null
            ? title
            : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);

        var price = product.Price;
        var compareAtPrice = product.CompareAtPrice;
        var categorySlug = ResolveCategoryTranslationValue(
            product.SubCategory.Category.Translations,
            languageId,
            defaultLanguageId,
            translation => translation.Slug);
        var subCategorySlug = ResolveSubCategoryTranslationValue(
            product.SubCategory.Translations,
            languageId,
            defaultLanguageId,
            translation => translation.Slug);

        var summary = new CatalogListingProductDto
        {
            Id = product.Id,
            Title = title,
            Slug = productSlug,
            ImageFileName = mainFile?.FileName ?? string.Empty,
            ImageAlt = imageAlt,
            Price = price,
            CompareAtPrice = compareAtPrice,
            OnSale = compareAtPrice.HasValue && compareAtPrice > price,
            InStock = product.InStock,
            CategorySlug = categorySlug,
            SubCategorySlug = subCategorySlug
        };

        var longDescriptions = await Context.ProductDescription
            .AsNoTracking()
            .Where(x => x.ProductId == product.Id && x.LanguageId == languageId)
            .Select(x => x.Description)
            .ToListAsync(cancellationToken);

        if (longDescriptions.Count == 0)
        {
            longDescriptions = await Context.ProductDescription
                .AsNoTracking()
                .Where(x => x.ProductId == product.Id && x.LanguageId == defaultLanguageId)
                .Select(x => x.Description)
                .ToListAsync(cancellationToken);
        }

        var images = product.ProductFiles
            .OrderByDescending(file => file.IsMain)
            .ThenBy(file => file.Id)
            .Select(file => new CatalogProductImageDto
            {
                Url = file.FileName,
                Alt = ResolveProductFileTranslation(file.Translations, languageId, defaultLanguageId)
            })
            .ToList();

        var features = await LoadCatalogFeaturesByProductIdAsync(
            product.Id,
            languageId,
            defaultLanguageId,
            cancellationToken);

        var rootComments = Context.ProductComment
            .AsNoTracking()
            .Where(comment =>
                comment.ProductId == product.Id
                && comment.IsActive
                && comment.ParentId == null);

        var reviewCount = await rootComments.CountAsync(cancellationToken);
        double? averageRating = null;
        int? satisfactionPercent = null;
        if (reviewCount > 0)
        {
            averageRating = await rootComments.AverageAsync(comment => (double)comment.CommentRate, cancellationToken);
            var satisfiedCount = await rootComments.CountAsync(comment => comment.CommentRate >= 4, cancellationToken);
            satisfactionPercent = (int)Math.Round((double)satisfiedCount / reviewCount * 100);
        }

        var purchaseCount = await Context.OrderItem
            .AsNoTracking()
            .Where(item =>
                item.ProductId == product.Id
                && (item.Order.Status == OrderStatusType.Completed
                    || item.Order.Status == OrderStatusType.InProgress))
            .SumAsync(item => item.Quantity, cancellationToken);

        return new CatalogProductDto
        {
            NotFound = false,
            Product = summary,
            Description = description,
            LongDescriptions = longDescriptions,
            Images = images,
            Features = features,
            ReviewCount = reviewCount,
            AverageRating = averageRating,
            SatisfactionPercent = satisfactionPercent,
            PurchaseCount = purchaseCount,
        };
    }

    public async Task<List<CatalogListingProductDto>> GetRelatedCatalogProductsBySlugAsync(
        string slug,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedSlug) || limit <= 0)
            return [];

        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var currentProduct = await Context.Product
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(
                x => x.Translations.Any(t => t.Slug.ToLower() == normalizedSlug),
                cancellationToken);

        if (currentProduct is null)
            return [];

        var rows = await Context.Product
            .AsNoTracking()
            .Where(product =>
                product.IsActive &&
                product.Id != currentProduct.Id &&
                product.SubCategoryId == currentProduct.SubCategoryId)
            .Include(product => product.Translations)
            .Include(product => product.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .OrderBy(product => product.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return rows.Select(product =>
        {
            var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
            var productSlug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
            var mainFile = product.ProductFiles
                .OrderByDescending(file => file.IsMain)
                .ThenBy(file => file.Id)
                .FirstOrDefault();
            var imageAlt = mainFile is null
                ? title
                : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);
            var price = product.Price;
            var compareAtPrice = product.CompareAtPrice;
            var categorySlug = ResolveCategoryTranslationValue(
                product.SubCategory.Category.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);
            var subCategorySlug = ResolveSubCategoryTranslationValue(
                product.SubCategory.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);

            return new CatalogListingProductDto
            {
                Id = product.Id,
                Title = title,
                Slug = productSlug,
                ImageFileName = mainFile?.FileName ?? string.Empty,
                ImageAlt = imageAlt,
                Price = price,
                CompareAtPrice = compareAtPrice,
                OnSale = compareAtPrice.HasValue && compareAtPrice > price,
                InStock = product.InStock,
                CategorySlug = categorySlug,
                SubCategorySlug = subCategorySlug
            };
        }).ToList();
    }

    public async Task<List<CatalogListingProductDto>> GetNewestCatalogProductsAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        if (limit <= 0)
            return [];

        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var rows = await Context.Product
            .AsNoTracking()
            .Where(product =>
                product.IsActive &&
                product.SubCategory.IsActive &&
                product.SubCategory.Category.IsActive)
            .Include(product => product.Translations)
            .Include(product => product.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .OrderByDescending(product => product.CreatedOnUtc)
            .ThenByDescending(product => product.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return rows.Select(product =>
        {
            var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
            var productSlug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
            var mainFile = product.ProductFiles
                .OrderByDescending(file => file.IsMain)
                .ThenBy(file => file.Id)
                .FirstOrDefault();
            var imageAlt = mainFile is null
                ? title
                : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);
            var price = product.Price;
            var compareAtPrice = product.CompareAtPrice;
            var categorySlug = ResolveCategoryTranslationValue(
                product.SubCategory.Category.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);
            var subCategorySlug = ResolveSubCategoryTranslationValue(
                product.SubCategory.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);

            return new CatalogListingProductDto
            {
                Id = product.Id,
                Title = title,
                Slug = productSlug,
                ImageFileName = mainFile?.FileName ?? string.Empty,
                ImageAlt = imageAlt,
                Price = price,
                CompareAtPrice = compareAtPrice,
                OnSale = compareAtPrice.HasValue && compareAtPrice > price,
                InStock = product.InStock,
                CategorySlug = categorySlug,
                SubCategorySlug = subCategorySlug,
            };
        }).ToList();
    }

    public Task<List<CatalogListingProductDto>> GetNewArrivalsCatalogProductsAsync(
        int? limit = null,
        CancellationToken cancellationToken = default)
        => LoadCategoryCollectionProductsAsync("new", limit, cancellationToken);

    public Task<List<CatalogListingProductDto>> GetInStockCatalogProductsAsync(
        int? limit = null,
        CancellationToken cancellationToken = default)
        => LoadFilteredCatalogProductsAsync(
            product => product.InStock,
            limit,
            cancellationToken);

    public async Task<List<CatalogListingProductDto>> GetBestSellingCatalogProductsAsync(
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var salesByProductId = await Context.OrderItem
            .AsNoTracking()
            .Where(item =>
                item.Order.Status == OrderStatusType.Completed
                || item.Order.Status == OrderStatusType.InProgress)
            .GroupBy(item => item.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                Sold = group.Sum(item => item.Quantity)
            })
            .ToDictionaryAsync(x => x.ProductId, x => x.Sold, cancellationToken);

        var rows = await BuildActiveCatalogProductQuery().ToListAsync(cancellationToken);

        IEnumerable<Product> ordered = rows
            .OrderByDescending(product => salesByProductId.GetValueOrDefault(product.Id))
            .ThenByDescending(product => product.CreatedOnUtc)
            .ThenByDescending(product => product.Id);

        if (limit is > 0)
            ordered = ordered.Take(limit.Value);

        return ordered
            .Select(product => MapToCatalogListingProductDto(product, languageId, defaultLanguageId))
            .ToList();
    }

    private async Task<List<CatalogListingProductDto>> LoadCategoryCollectionProductsAsync(
        string categorySlug,
        int? limit,
        CancellationToken cancellationToken)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var categoryId = await Context.CategoryTranslation
            .AsNoTracking()
            .Where(translation => translation.Slug.ToLower() == categorySlug)
            .Select(translation => translation.CategoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (categoryId == default)
            return [];

        var subCategoryIds = await Context.SubCategory
            .AsNoTracking()
            .Where(subCategory => subCategory.IsActive && subCategory.CategoryId == categoryId)
            .Select(subCategory => subCategory.Id)
            .ToListAsync(cancellationToken);

        if (subCategoryIds.Count == 0)
            return [];

        IQueryable<Product> query = BuildActiveCatalogProductQuery()
            .Where(product => subCategoryIds.Contains(product.SubCategoryId))
            .OrderByDescending(product => product.CreatedOnUtc)
            .ThenByDescending(product => product.Id);

        if (limit is > 0)
            query = query.Take(limit.Value);

        var rows = await query.ToListAsync(cancellationToken);

        return rows
            .Select(product => MapToCatalogListingProductDto(product, languageId, defaultLanguageId))
            .ToList();
    }

    private async Task<List<CatalogListingProductDto>> LoadFilteredCatalogProductsAsync(
        Expression<Func<Product, bool>> predicate,
        int? limit,
        CancellationToken cancellationToken)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        IQueryable<Product> query = BuildActiveCatalogProductQuery()
            .Where(predicate)
            .OrderByDescending(product => product.CreatedOnUtc)
            .ThenByDescending(product => product.Id);

        if (limit is > 0)
            query = query.Take(limit.Value);

        var rows = await query.ToListAsync(cancellationToken);

        return rows
            .Select(product => MapToCatalogListingProductDto(product, languageId, defaultLanguageId))
            .ToList();
    }

    private IQueryable<Product> BuildActiveCatalogProductQuery()
        => Context.Product
            .AsNoTracking()
            .Where(product =>
                product.IsActive
                && product.SubCategory.IsActive
                && product.SubCategory.Category.IsActive)
            .Include(product => product.Translations)
            .Include(product => product.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations);

    private static CatalogListingProductDto MapToCatalogListingProductDto(
        Product product,
        int languageId,
        int defaultLanguageId)
    {
        var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
        var productSlug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
        var mainFile = product.ProductFiles
            .OrderByDescending(file => file.IsMain)
            .ThenBy(file => file.Id)
            .FirstOrDefault();
        var imageAlt = mainFile is null
            ? title
            : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);
        var price = product.Price;
        var compareAtPrice = product.CompareAtPrice;
        var categorySlug = ResolveCategoryTranslationValue(
            product.SubCategory.Category.Translations,
            languageId,
            defaultLanguageId,
            translation => translation.Slug);
        var subCategorySlug = ResolveSubCategoryTranslationValue(
            product.SubCategory.Translations,
            languageId,
            defaultLanguageId,
            translation => translation.Slug);

        return new CatalogListingProductDto
        {
            Id = product.Id,
            Title = title,
            Slug = productSlug,
            ImageFileName = mainFile?.FileName ?? string.Empty,
            ImageAlt = imageAlt,
            Price = price,
            CompareAtPrice = compareAtPrice,
            OnSale = compareAtPrice.HasValue && compareAtPrice > price,
            InStock = product.InStock,
            IsNew = CatalogListingFilterProcessor.IsNewProduct(product.CreatedOnUtc),
            CategorySlug = categorySlug,
            SubCategorySlug = subCategorySlug,
        };
    }

    private async Task<string> ResolveCollectionLabelAsync(
        string collectionSlug,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var language = await languageRegistry.GetByIdAsync(languageId, cancellationToken)
                       ?? await languageRegistry.GetByIdAsync(defaultLanguageId, cancellationToken);
        var isFa = (language?.Code ?? "en-US").StartsWith("fa", StringComparison.OrdinalIgnoreCase);

        return collectionSlug switch
        {
            "in-stock" => isFa ? "موجود و آماده ارسال" : "In Stock & Ready to Ship",
            "best-sellers" => isFa ? "پرفروش‌ها" : "Best Sellers",
            "sale" => isFa ? "حراج" : "Sale",
            "new" => isFa ? "محصولات جدید" : "New Arrivals",
            "clearance" => isFa ? "تخفیف ویژه" : "Clearance",
            _ => collectionSlug
        };
    }

    private async Task<List<CatalogListingProductDto>> LoadCatalogProductsAsync(
        IReadOnlyCollection<int> subCategoryIds,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        if (subCategoryIds.Count == 0)
            return [];

        var rows = await Context.Product
            .AsNoTracking()
            .Where(product => product.IsActive && subCategoryIds.Contains(product.SubCategoryId))
            .Include(product => product.Translations)
            .Include(product => product.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .OrderBy(product => product.Id)
            .ToListAsync(cancellationToken);

        return rows
            .Select(product => MapToCatalogListingProductDto(product, languageId, defaultLanguageId))
            .ToList();
    }

    private static string ResolveProductTranslation(
        IEnumerable<ProductTranslation> translations,
        int languageId,
        int defaultLanguageId,
        Func<ProductTranslation, string> selector)
    {
        var list = translations as IList<ProductTranslation> ?? translations.ToList();
        var translation = list.FirstOrDefault(t => t.LanguageId == languageId)
                          ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)
                          ?? list.FirstOrDefault();
        return translation is null ? string.Empty : selector(translation);
    }

    private static string ResolveProductFileTranslation(
        IEnumerable<ProductFileTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<ProductFileTranslation> ?? translations.ToList();
        return list.FirstOrDefault(t => t.LanguageId == languageId)?.Title
               ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)?.Title
               ?? list.FirstOrDefault()?.Title
               ?? string.Empty;
    }

    private static string ResolveCategoryTranslationValue(
        IEnumerable<CategoryTranslation> translations,
        int languageId,
        int defaultLanguageId,
        Func<CategoryTranslation, string> selector)
    {
        var list = translations as IList<CategoryTranslation> ?? translations.ToList();
        var translation = list.FirstOrDefault(t => t.LanguageId == languageId)
                          ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)
                          ?? list.FirstOrDefault();
        return translation is null ? string.Empty : selector(translation);
    }

    private static string ResolveSubCategoryTranslationValue(
        IEnumerable<SubCategoryTranslation> translations,
        int languageId,
        int defaultLanguageId,
        Func<SubCategoryTranslation, string> selector)
    {
        var list = translations as IList<SubCategoryTranslation> ?? translations.ToList();
        var translation = list.FirstOrDefault(t => t.LanguageId == languageId)
                          ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)
                          ?? list.FirstOrDefault();
        return translation is null ? string.Empty : selector(translation);
    }

    private async Task<(string Title, string Slug)> ResolveCategoryTranslationAsync(
        int categoryId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var translations = await Context.CategoryTranslation
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync(cancellationToken);

        var current = translations.FirstOrDefault(x => x.LanguageId == languageId)
                      ?? translations.FirstOrDefault(x => x.LanguageId == defaultLanguageId)
                      ?? translations.FirstOrDefault();

        return current is null
            ? (string.Empty, string.Empty)
            : (current.Title, current.Slug);
    }

    private async Task<(string Title, string Slug)> ResolveSubCategoryTranslationAsync(
        int subCategoryId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var translations = await Context.SubCategoryTranslation
            .AsNoTracking()
            .Where(x => x.SubCategoryId == subCategoryId)
            .ToListAsync(cancellationToken);

        var current = translations.FirstOrDefault(x => x.LanguageId == languageId)
                      ?? translations.FirstOrDefault(x => x.LanguageId == defaultLanguageId)
                      ?? translations.FirstOrDefault();

        return current is null
            ? (string.Empty, string.Empty)
            : (current.Title, current.Slug);
    }

    private async Task<string> ResolveHomeLabelAsync(
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var language = await languageRegistry.GetByIdAsync(languageId, cancellationToken)
                       ?? await languageRegistry.GetByIdAsync(defaultLanguageId, cancellationToken);
        var code = language?.Code ?? "en-US";
        return code.StartsWith("fa", StringComparison.OrdinalIgnoreCase) ? "خانه" : "Home";
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken = default)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }

    private async Task<List<CatalogProductFeatureDto>> LoadCatalogFeaturesByProductIdAsync(
        int productId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        return await (
                from productProperty in Context.ProductProperty.AsNoTracking()
                where productProperty.ProductId == productId && productProperty.IsActive
                join property in Context.Property.AsNoTracking() on productProperty.PropertyId equals property.Id
                where property.IsActive && property.ParentId == null
                join propertyCategory in Context.PropertyCategory.AsNoTracking() on property.PropertyCategoryId equals propertyCategory.Id
                where propertyCategory.IsActive
                join propertyItem in Context.PropertyItem.AsNoTracking() on productProperty.PropertyItemId equals propertyItem.Id into propertyItems
                from propertyItem in propertyItems.DefaultIfEmpty()
                where productProperty.PropertyItemId == null || propertyItem.IsActive
                orderby property.Priority, property.Id
                select new CatalogProductFeatureDto
                {
                    Label = property.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                        ?? property.Translations
                            .Where(t => t.LanguageId == defaultLanguageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                        ?? string.Empty,
                    Value = productProperty.PropertyItemId == null
                        ? string.Empty
                        : propertyItem.Translations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                            ?? propertyItem.Translations
                                .Where(t => t.LanguageId == defaultLanguageId)
                                .Select(t => t.Title)
                                .FirstOrDefault()
                            ?? string.Empty,
                    GroupTitle = propertyCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                        ?? propertyCategory.Translations
                            .Where(t => t.LanguageId == defaultLanguageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                })
            .Where(feature => !string.IsNullOrWhiteSpace(feature.Label) && !string.IsNullOrWhiteSpace(feature.Value))
            .ToListAsync(cancellationToken);
    }

    private static readonly HashSet<string> CollectionSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        "sale", "new", "in-stock", "best-sellers", "clearance",
    };

    private static bool TryParseSplitCategoryPath(
        string normalizedPath,
        out string categorySegment,
        out string subCategorySegment)
    {
        var segments = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 2)
        {
            categorySegment = segments[0];
            subCategorySegment = segments[1];
            return true;
        }

        categorySegment = string.Empty;
        subCategorySegment = string.Empty;
        return false;
    }

    private static string BuildSubCategoryListingPath(string categorySlug, string subCategorySlug)
    {
        var normalizedCategory = categorySlug.Trim().Trim('/');
        var normalizedSub = subCategorySlug.Trim().Trim('/');
        if (string.IsNullOrWhiteSpace(normalizedSub))
            return normalizedCategory;
        if (normalizedSub.Contains('/'))
            return normalizedSub;
        if (string.IsNullOrWhiteSpace(normalizedCategory))
            return normalizedSub;

        return $"{normalizedCategory}/{normalizedSub}";
    }

    private async Task<int> FindActiveCategoryIdByNormalizedSlugAsync(
        string normalizedSlug,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(normalizedSlug))
            return default;

        var rows = await (
                from translation in Context.CategoryTranslation.AsNoTracking()
                join category in Context.Category.AsNoTracking() on translation.CategoryId equals category.Id
                where category.IsActive
                select new { category.Id, translation.Slug, translation.LanguageId })
            .ToListAsync(cancellationToken);

        return rows
            .Where(x => CatalogSlugNormalizer.Normalize(x.Slug) == normalizedSlug)
            .OrderBy(x => x.LanguageId == languageId ? 0 : x.LanguageId == defaultLanguageId ? 1 : 2)
            .Select(x => x.Id)
            .FirstOrDefault();
    }

    private async Task<(int SubCategoryId, int CategoryId)?> FindActiveSubCategoryByNormalizedSlugAsync(
        string normalizedSlug,
        int? categoryId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(normalizedSlug))
            return null;

        var rows = await (
                from translation in Context.SubCategoryTranslation.AsNoTracking()
                join subCategory in Context.SubCategory.AsNoTracking() on translation.SubCategoryId equals subCategory.Id
                join category in Context.Category.AsNoTracking() on subCategory.CategoryId equals category.Id
                where subCategory.IsActive && category.IsActive
                select new
                {
                    subCategory.Id,
                    subCategory.CategoryId,
                    translation.Slug,
                    translation.LanguageId,
                })
            .ToListAsync(cancellationToken);

        IEnumerable<(int Id, int CategoryId, string Slug, int LanguageId)> candidates = rows
            .Select(x => (x.Id, x.CategoryId, x.Slug, x.LanguageId));

        if (categoryId is not null)
            candidates = candidates.Where(x => x.CategoryId == categoryId);

        var match = candidates
            .Where(x => CatalogSlugNormalizer.Normalize(x.Slug) == normalizedSlug)
            .OrderBy(x => x.LanguageId == languageId ? 0 : x.LanguageId == defaultLanguageId ? 1 : 2)
            .Select(x => ((int SubCategoryId, int CategoryId)?)(x.Id, x.CategoryId))
            .FirstOrDefault();

        if (match is not null)
            return match;

        var leafSlug = normalizedSlug.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        if (string.IsNullOrWhiteSpace(leafSlug) || leafSlug == normalizedSlug)
            return null;

        return candidates
            .Where(x =>
            {
                var normalized = CatalogSlugNormalizer.Normalize(x.Slug);
                return normalized == leafSlug || normalized.EndsWith("/" + leafSlug, StringComparison.Ordinal);
            })
            .OrderBy(x => x.LanguageId == languageId ? 0 : x.LanguageId == defaultLanguageId ? 1 : 2)
            .Select(x => ((int SubCategoryId, int CategoryId)?)(x.Id, x.CategoryId))
            .FirstOrDefault();
    }

    private async Task<(int SubCategoryId, int CategoryId)?> TryMatchCategorySubCategorySegmentsAsync(
        string categorySegment,
        string subCategorySegment,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var normalizedCategory = CatalogSlugNormalizer.Normalize(categorySegment);
        var normalizedSub = CatalogSlugNormalizer.Normalize(subCategorySegment);
        if (string.IsNullOrWhiteSpace(normalizedCategory) || string.IsNullOrWhiteSpace(normalizedSub))
            return null;

        var categoryId = await FindActiveCategoryIdByNormalizedSlugAsync(
            normalizedCategory,
            languageId,
            defaultLanguageId,
            cancellationToken);

        if (categoryId == default)
            return null;

        var compositePath = CatalogSlugNormalizer.Normalize($"{normalizedCategory}/{normalizedSub}");

        return await FindActiveSubCategoryByNormalizedSlugAsync(
                   compositePath,
                   categoryId,
                   languageId,
                   defaultLanguageId,
                   cancellationToken)
               ?? await FindActiveSubCategoryByNormalizedSlugAsync(
                   normalizedSub,
                   categoryId,
                   languageId,
                   defaultLanguageId,
                   cancellationToken);
    }

    private async Task<CatalogListingDto> BuildSubCategoryListingAsync(
        int subCategoryId,
        int categoryId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var categoryTranslation = await ResolveCategoryTranslationAsync(
            categoryId,
            languageId,
            defaultLanguageId,
            cancellationToken);
        var subCategoryTranslation = await ResolveSubCategoryTranslationAsync(
            subCategoryId,
            languageId,
            defaultLanguageId,
            cancellationToken);
        var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
        var listingPath = BuildSubCategoryListingPath(categoryTranslation.Slug, subCategoryTranslation.Slug);
        var products = await LoadCatalogProductsAsync(
            [subCategoryId],
            languageId,
            defaultLanguageId,
            cancellationToken);

        return new CatalogListingDto
        {
            Title = subCategoryTranslation.Title,
            Breadcrumbs =
            [
                new(homeLabel, "/"),
                new(categoryTranslation.Title, $"/{categoryTranslation.Slug.Trim('/')}"),
                new(subCategoryTranslation.Title, $"/{listingPath}"),
            ],
            Products = products,
        };
    }

    private static (string? Collection, string RemainingPath) ParseCollectionPrefix(string normalizedPath)
    {
        var segments = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0 || !CollectionSlugs.Contains(segments[0]))
            return (null, normalizedPath);

        var collection = segments[0].ToLowerInvariant();
        var remaining = segments.Length > 1 ? string.Join('/', segments.Skip(1)) : string.Empty;
        return (collection, remaining);
    }

    private async Task<CatalogListingDto> WithEnrichedProductsAsync(
        CatalogListingDto listing,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        if (!listing.PathNotFound && listing.Products.Count > 0)
            await EnrichListingProductsAsync(listing, languageId, defaultLanguageId, cancellationToken);

        return listing;
    }

    private async Task<CatalogListingDto> BuildCollectionListingAsync(
        string collectionSlug,
        string categoryPath,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
        var collectionTitle = await ResolveCollectionLabelAsync(
            collectionSlug,
            languageId,
            defaultLanguageId,
            cancellationToken);

        List<CatalogListingProductDto> products;
        List<CatalogBreadcrumbDto> breadcrumbs =
        [
            new(homeLabel, "/"),
            new(collectionTitle, $"/{collectionSlug}"),
        ];

        if (string.IsNullOrWhiteSpace(categoryPath))
        {
            products = collectionSlug switch
            {
                "sale" or "clearance" => await LoadFilteredCatalogProductsAsync(
                    product => product.CompareAtPrice != null && product.CompareAtPrice > product.Price,
                    null,
                    cancellationToken),
                "new" => await LoadFilteredCatalogProductsAsync(
                    product => product.CreatedOnUtc >= DateTime.UtcNow.AddDays(-90),
                    null,
                    cancellationToken),
                "in-stock" => await GetInStockCatalogProductsAsync(cancellationToken: cancellationToken),
                "best-sellers" => await GetBestSellingCatalogProductsAsync(cancellationToken: cancellationToken),
                _ => [],
            };
        }
        else
        {
            var scopedListing = await ResolveScopedCategoryListingAsync(
                categoryPath,
                languageId,
                defaultLanguageId,
                cancellationToken);

            if (scopedListing.PathNotFound)
                return scopedListing;

            await EnrichListingProductsAsync(
                scopedListing,
                languageId,
                defaultLanguageId,
                cancellationToken);

            products = collectionSlug switch
            {
                "sale" or "clearance" => scopedListing.Products.Where(product => product.OnSale).ToList(),
                "new" => scopedListing.Products.Where(product => product.IsNew).ToList(),
                "in-stock" => scopedListing.Products.Where(product => product.InStock).ToList(),
                "best-sellers" => scopedListing.Products
                    .OrderByDescending(product => product.PurchaseCount)
                    .ThenByDescending(product => product.Id)
                    .ToList(),
                _ => scopedListing.Products,
            };

            breadcrumbs =
            [
                new(homeLabel, "/"),
                new(collectionTitle, $"/{collectionSlug}"),
                .. scopedListing.Breadcrumbs.Skip(1),
            ];
            collectionTitle = $"{collectionTitle} — {scopedListing.Title}";
        }

        return new CatalogListingDto
        {
            Title = collectionTitle,
            Breadcrumbs = breadcrumbs,
            Products = products,
        };
    }

    private async Task<CatalogListingDto> ResolveScopedCategoryListingAsync(
        string categoryPath,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var normalizedPath = CatalogSlugNormalizer.NormalizePath(categoryPath);

        var subCategoryMatch = await FindActiveSubCategoryByNormalizedSlugAsync(
            normalizedPath,
            categoryId: null,
            languageId,
            defaultLanguageId,
            cancellationToken);

        if (subCategoryMatch is not null)
        {
            return await BuildSubCategoryListingAsync(
                subCategoryMatch.Value.SubCategoryId,
                subCategoryMatch.Value.CategoryId,
                languageId,
                defaultLanguageId,
                cancellationToken);
        }

        if (TryParseSplitCategoryPath(normalizedPath, out var categorySegment, out var subCategorySegment))
        {
            var splitMatch = await TryMatchCategorySubCategorySegmentsAsync(
                categorySegment,
                subCategorySegment,
                languageId,
                defaultLanguageId,
                cancellationToken);

            if (splitMatch is not null)
            {
                return await BuildSubCategoryListingAsync(
                    splitMatch.Value.SubCategoryId,
                    splitMatch.Value.CategoryId,
                    languageId,
                    defaultLanguageId,
                    cancellationToken);
            }
        }

        var categoryMatch = await FindActiveCategoryIdByNormalizedSlugAsync(
            normalizedPath,
            languageId,
            defaultLanguageId,
            cancellationToken);

        if (categoryMatch == default)
        {
            return new CatalogListingDto
            {
                PathNotFound = true,
                Title = normalizedPath,
            };
        }

        var categoryTranslationOnly = await ResolveCategoryTranslationAsync(
            categoryMatch,
            languageId,
            defaultLanguageId,
            cancellationToken);
        var subCategoryIds = await Context.SubCategory
            .AsNoTracking()
            .Where(x => x.IsActive && x.CategoryId == categoryMatch)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
        var home = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
        var categoryProducts = await LoadCatalogProductsAsync(
            subCategoryIds,
            languageId,
            defaultLanguageId,
            cancellationToken);

        return new CatalogListingDto
        {
            Title = categoryTranslationOnly.Title,
            Breadcrumbs =
            [
                new(home, "/"),
                new(categoryTranslationOnly.Title, $"/{categoryTranslationOnly.Slug}"),
            ],
            Products = categoryProducts,
        };
    }

    private async Task EnrichListingProductsAsync(
        CatalogListingDto listing,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var products = listing.Products;
        if (products.Count == 0)
            return;

        var productIds = products.Select(product => product.Id).Distinct().ToList();
        var facetBatch = await LoadProductFacetsWithLabelsBatchAsync(
            productIds,
            languageId,
            defaultLanguageId,
            cancellationToken);
        var reviewStats = await LoadProductReviewStatsBatchAsync(productIds, cancellationToken);
        var purchaseCounts = await LoadProductPurchaseCountsBatchAsync(productIds, cancellationToken);

        listing.FacetLabels.GroupLabels.Clear();
        foreach (var (key, label) in facetBatch.GroupLabels)
            listing.FacetLabels.GroupLabels[key] = label;

        listing.FacetLabels.OptionLabels.Clear();
        foreach (var (facetKey, options) in facetBatch.OptionLabels)
            listing.FacetLabels.OptionLabels[facetKey] = options;

        foreach (var product in products)
        {
            if (facetBatch.FacetsByProductId.TryGetValue(product.Id, out var facets))
            {
                product.Facets.Clear();
                foreach (var (key, values) in facets)
                    product.Facets[key] = values;
            }

            if (reviewStats.TryGetValue(product.Id, out var review))
            {
                product.ReviewCount = review.ReviewCount;
                product.AverageRating = review.AverageRating;
            }

            if (purchaseCounts.TryGetValue(product.Id, out var sold))
                product.PurchaseCount = sold;
        }
    }

    private sealed record ProductFacetBatchResult(
        Dictionary<int, Dictionary<string, List<string>>> FacetsByProductId,
        Dictionary<string, string> GroupLabels,
        Dictionary<string, Dictionary<string, string>> OptionLabels);

    private async Task<ProductFacetBatchResult> LoadProductFacetsWithLabelsBatchAsync(
        IReadOnlyCollection<int> productIds,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return new ProductFacetBatchResult(
                new Dictionary<int, Dictionary<string, List<string>>>(),
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase));
        }

        var rows = await (
                from productProperty in Context.ProductProperty.AsNoTracking()
                where productIds.Contains(productProperty.ProductId) && productProperty.IsActive
                join property in Context.Property.AsNoTracking() on productProperty.PropertyId equals property.Id
                where property.IsActive && property.ParentId == null
                join propertyCategory in Context.PropertyCategory.AsNoTracking()
                    on property.PropertyCategoryId equals propertyCategory.Id
                where propertyCategory.IsActive
                join propertyItem in Context.PropertyItem.AsNoTracking()
                    on productProperty.PropertyItemId equals propertyItem.Id into propertyItems
                from propertyItem in propertyItems.DefaultIfEmpty()
                where productProperty.PropertyItemId == null || propertyItem.IsActive
                select new
                {
                    productProperty.ProductId,
                    FacetKey = propertyCategory.Code.ToLower(),
                    GroupLabel = propertyCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                        ?? propertyCategory.Translations
                            .Where(t => t.LanguageId == defaultLanguageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                        ?? propertyCategory.Code,
                    Value = productProperty.PropertyItemId == null
                        ? string.Empty
                        : propertyItem.Code.ToLower(),
                    OptionLabel = productProperty.PropertyItemId == null
                        ? string.Empty
                        : propertyItem.Translations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                            ?? propertyItem.Translations
                                .Where(t => t.LanguageId == defaultLanguageId)
                                .Select(t => t.Title)
                                .FirstOrDefault()
                            ?? propertyItem.Code,
                })
            .Where(row => !string.IsNullOrWhiteSpace(row.Value))
            .ToListAsync(cancellationToken);

        var facetsByProductId = new Dictionary<int, Dictionary<string, List<string>>>();
        var groupLabels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var optionLabels = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            if (!string.IsNullOrWhiteSpace(row.GroupLabel))
                groupLabels[row.FacetKey] = row.GroupLabel;

            if (!optionLabels.TryGetValue(row.FacetKey, out var facetOptions))
            {
                facetOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                optionLabels[row.FacetKey] = facetOptions;
            }

            if (!string.IsNullOrWhiteSpace(row.OptionLabel))
                facetOptions[row.Value] = row.OptionLabel;

            if (!facetsByProductId.TryGetValue(row.ProductId, out var facets))
            {
                facets = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                facetsByProductId[row.ProductId] = facets;
            }

            if (!facets.TryGetValue(row.FacetKey, out var values))
            {
                values = [];
                facets[row.FacetKey] = values;
            }

            if (!values.Contains(row.Value, StringComparer.OrdinalIgnoreCase))
                values.Add(row.Value);
        }

        return new ProductFacetBatchResult(facetsByProductId, groupLabels, optionLabels);
    }

    private async Task<Dictionary<int, (int ReviewCount, double? AverageRating)>> LoadProductReviewStatsBatchAsync(
        IReadOnlyCollection<int> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
            return new Dictionary<int, (int ReviewCount, double? AverageRating)>();

        var rows = await Context.ProductComment
            .AsNoTracking()
            .Where(comment =>
                productIds.Contains(comment.ProductId)
                && comment.IsActive
                && comment.ParentId == null)
            .GroupBy(comment => comment.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                ReviewCount = group.Count(),
                AverageRating = group.Average(comment => (double)comment.CommentRate),
            })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(
            row => row.ProductId,
            row => (row.ReviewCount, (double?)row.AverageRating));
    }

    private async Task<Dictionary<int, int>> LoadProductPurchaseCountsBatchAsync(
        IReadOnlyCollection<int> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
            return new Dictionary<int, int>();

        return await Context.OrderItem
            .AsNoTracking()
            .Where(item =>
                productIds.Contains(item.ProductId)
                && (item.Order.Status == OrderStatusType.Completed
                    || item.Order.Status == OrderStatusType.InProgress))
            .GroupBy(item => item.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                Sold = group.Sum(item => item.Quantity),
            })
            .ToDictionaryAsync(row => row.ProductId, row => row.Sold, cancellationToken);
    }
}
