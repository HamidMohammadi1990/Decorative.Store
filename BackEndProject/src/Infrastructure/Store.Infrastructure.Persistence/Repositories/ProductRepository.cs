using System.Linq.Expressions;
using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

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
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from product in productSource
            join subCategory in Context.SubCategory on product.SubCategoryId equals subCategory.Id
            join category in Context.Category on subCategory.CategoryId equals category.Id
            join companyProduct in Context.CompanyProduct on product.Id equals companyProduct.ProductId
            select new { product, subCategory, category, companyProduct };

        if (request.CompanyId.HasValue)
            query = query.Where(x => x.companyProduct.CompanyId == request.CompanyId.Value);

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
            .Distinct()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken = default)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
