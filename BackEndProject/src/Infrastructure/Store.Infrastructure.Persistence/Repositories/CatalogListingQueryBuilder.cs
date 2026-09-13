using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Catalog;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Repositories;

internal static class CatalogListingQueryBuilder
{
    private static readonly TimeSpan NewProductWindow = TimeSpan.FromDays(90);

    private static readonly (string Id, decimal Min, decimal Max)[] PriceBuckets =
    [
        ("under-5m", 0, 4_999_999),
        ("5m-10m", 5_000_000, 9_999_999),
        ("10m-15m", 10_000_000, 14_999_999),
        ("15m-20m", 15_000_000, 19_999_999),
        ("over-20m", 20_000_000, decimal.MaxValue),
    ];

    public static (int Page, int PageSize) NormalizePaging(CatalogListingQueryDto query)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 12 : Math.Min(query.PageSize, 48);
        return (page, pageSize);
    }

    public static IQueryable<Product> BuildLanguageScopedQuery(
        EditionDbContext context,
        IReadOnlyCollection<int> subCategoryIds,
        int languageId)
    {
        if (subCategoryIds.Count == 0)
            return context.Product.Where(_ => false);

        return context.Product
            .AsNoTracking()
            .Where(product =>
                product.IsActive
                && subCategoryIds.Contains(product.SubCategoryId)
                && product.SubCategory!.IsActive
                && product.SubCategory.Category!.IsActive
                && product.Translations.Any(translation => translation.LanguageId == languageId));
    }

    public static IQueryable<Product> ApplyFilters(
        EditionDbContext context,
        IQueryable<Product> query,
        CatalogListingQueryDto filters)
    {
        if (filters.InStock == true)
            query = query.Where(product => product.InStock);

        if (filters.OnSale == true)
            query = query.Where(product => product.CompareAtPrice != null && product.CompareAtPrice > product.Price);

        if (filters.IsNew == true)
        {
            var cutoff = DateTime.UtcNow.Subtract(NewProductWindow);
            query = query.Where(product => product.CreatedOnUtc >= cutoff);
        }

        if (filters.MinRating is > 0)
        {
            var minRating = filters.MinRating.Value;
            query = query.Where(product =>
                context.ProductComment
                    .Where(comment =>
                        comment.ProductId == product.Id
                        && comment.IsActive
                        && comment.ParentId == null)
                    .Average(comment => (double?)comment.CommentRate) >= minRating);
        }

        query = ApplyPriceFilters(query, filters);

        foreach (var (facetId, values) in filters.AttributeFilters)
        {
            if (values.Count == 0)
                continue;

            var normalizedFacetId = facetId.ToLowerInvariant();
            var normalizedValues = values
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.ToLowerInvariant())
                .Distinct()
                .ToList();

            if (normalizedValues.Count == 0)
                continue;

            query = query.Where(product =>
                product.ProductProperties.Any(productProperty =>
                    productProperty.IsActive
                    && productProperty.PropertyItemId != null
                    && productProperty.Property.IsActive
                    && productProperty.Property.ParentId == null
                    && productProperty.Property.PropertyCategory!.IsActive
                    && productProperty.Property.PropertyCategory.Code.ToLower() == normalizedFacetId
                    && productProperty.PropertyItem!.IsActive
                    && normalizedValues.Contains(productProperty.PropertyItem.Code.ToLower())));
        }

        return query;
    }

    public static IQueryable<Product> ApplySort(
        EditionDbContext context,
        IQueryable<Product> query,
        string? sort)
    {
        // Compute once outside the expression tree — EF cannot translate DateTime.UtcNow.Subtract(...).
        var newProductCutoff = DateTime.UtcNow.Subtract(NewProductWindow);

        return sort switch
        {
            "price-asc" => query.OrderBy(product => product.Price).ThenBy(product => product.Id),
            "price-desc" => query.OrderByDescending(product => product.Price).ThenBy(product => product.Id),
            "newest" => query
                .OrderByDescending(product => product.CreatedOnUtc >= newProductCutoff)
                .ThenByDescending(product => product.Id),
            "best-selling" => query
                .OrderByDescending(product =>
                    context.OrderItem
                        .Where(item =>
                            item.ProductId == product.Id
                            && (item.Order.Status == OrderStatusType.Completed
                                || item.Order.Status == OrderStatusType.InProgress))
                        .Sum(item => (int?)item.Quantity) ?? 0)
                .ThenByDescending(product => product.Id),
            "rating" => query
                .OrderByDescending(product =>
                    context.ProductComment
                        .Where(comment =>
                            comment.ProductId == product.Id
                            && comment.IsActive
                            && comment.ParentId == null)
                        .Average(comment => (double?)comment.CommentRate) ?? 0)
                .ThenByDescending(product =>
                    context.ProductComment
                        .Count(comment =>
                            comment.ProductId == product.Id
                            && comment.IsActive
                            && comment.ParentId == null))
                .ThenBy(product => product.Id),
            _ => query.OrderBy(product => product.Id),
        };
    }

    public static IQueryable<CatalogListingProductDto> ProjectListingProducts(
        IQueryable<Product> query,
        int languageId)
    {
        var newProductCutoff = DateTime.UtcNow.Subtract(NewProductWindow);

        return query.Select(product => new CatalogListingProductDto
        {
            Id = product.Id,
            Title = product.Translations
                .Where(translation => translation.LanguageId == languageId)
                .Select(translation => translation.Title)
                .FirstOrDefault() ?? string.Empty,
            Slug = product.Translations
                .Where(translation => translation.LanguageId == languageId)
                .Select(translation => translation.Slug)
                .FirstOrDefault() ?? string.Empty,
            ImageFileName = product.ProductFiles
                .Where(file => file.IsActive && file.FileTypeId == ProductFileKind.Gallery)
                .OrderByDescending(file => file.IsMain)
                .ThenBy(file => file.Id)
                .Select(file => file.FileName)
                .FirstOrDefault() ?? string.Empty,
            ImageAlt = product.ProductFiles
                .Where(file => file.IsActive && file.FileTypeId == ProductFileKind.Gallery)
                .OrderByDescending(file => file.IsMain)
                .ThenBy(file => file.Id)
                .Select(file => file.Translations
                    .Where(translation => translation.LanguageId == languageId)
                    .Select(translation => translation.Title)
                    .FirstOrDefault())
                .FirstOrDefault()
                ?? product.Translations
                    .Where(translation => translation.LanguageId == languageId)
                    .Select(translation => translation.Title)
                    .FirstOrDefault()
                ?? string.Empty,
            Price = product.Price,
            CompareAtPrice = product.CompareAtPrice,
            OnSale = product.CompareAtPrice != null && product.CompareAtPrice > product.Price,
            InStock = product.InStock,
            IsNew = product.CreatedOnUtc >= newProductCutoff,
            CategorySlug = product.SubCategory!.Category!.Translations
                .Where(translation => translation.LanguageId == languageId)
                .Select(translation => translation.Slug)
                .FirstOrDefault() ?? string.Empty,
            SubCategorySlug = product.SubCategory!.Translations
                .Where(translation => translation.LanguageId == languageId)
                .Select(translation => translation.Slug)
                .FirstOrDefault() ?? string.Empty,
        });
    }

    public static IQueryable<CatalogListingProductDto> ProjectFacetScopeProducts(IQueryable<Product> query)
    {
        var newProductCutoff = DateTime.UtcNow.Subtract(NewProductWindow);

        return query.Select(product => new CatalogListingProductDto
        {
            Id = product.Id,
            Price = product.Price,
            CompareAtPrice = product.CompareAtPrice,
            OnSale = product.CompareAtPrice != null && product.CompareAtPrice > product.Price,
            InStock = product.InStock,
            IsNew = product.CreatedOnUtc >= newProductCutoff,
        });
    }

    private static IQueryable<Product> ApplyPriceFilters(
        IQueryable<Product> query,
        CatalogListingQueryDto filters)
    {
        if (filters.PriceBuckets.Count > 0)
        {
            var activeBuckets = PriceBuckets
                .Where(bucket => filters.PriceBuckets.Contains(bucket.Id, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (activeBuckets.Count > 0)
            {
                query = query.Where(product =>
                    activeBuckets.Any(bucket =>
                        product.Price >= bucket.Min && product.Price <= bucket.Max));
            }
        }

        if (filters.MinPrice is not null)
            query = query.Where(product => product.Price >= filters.MinPrice.Value);

        if (filters.MaxPrice is not null)
            query = query.Where(product => product.Price <= filters.MaxPrice.Value);

        return query;
    }
}
