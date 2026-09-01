using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFiles;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductFileRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<ProductFile>(context), IProductFileRepository
{
    public Task<ProductFile?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.ProductFile
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ProductFile?> GetWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.ProductFile
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task ClearMainFlagsAsync(int productId, CancellationToken cancellationToken = default)
    {
        var mainFiles = await Context.ProductFile
            .Where(x => x.ProductId == productId && x.IsMain)
            .ToListAsync(cancellationToken);

        foreach (var file in mainFiles)
            file.SetMain(false);
    }

    public async Task<PagedResult<GetAllProductFileResponseDto>> GetAllAsync(
        GetAllProductFileRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var productFileSource = Context.ProductFile
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productFiles =
            from productFile in productFileSource
            join product in Context.Product on productFile.ProductId equals product.Id
            select new { productFile, product };

        if (request.ProductId.HasValue)
            productFiles = productFiles.Where(x => x.productFile.ProductId == request.ProductId.Value);

        if (request.IsActive.HasValue)
            productFiles = productFiles.Where(x => x.productFile.IsActive == request.IsActive.Value);

        if (request.IsMain.HasValue)
            productFiles = productFiles.Where(x => x.productFile.IsMain == request.IsMain.Value);

        if (!string.IsNullOrWhiteSpace(request.Title))
            productFiles = productFiles.Where(x => x.productFile.Translations.Any(t => t.Title.Contains(request.Title)));

        var result = await productFiles
            .Select(x => new GetAllProductFileResponseDto
            {
                Id = x.productFile.Id,
                IsMain = x.productFile.IsMain,
                IsActive = x.productFile.IsActive,
                FileName = x.productFile.FileName,
                ProductId = x.productFile.ProductId,
                Title = x.productFile.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.productFile.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                ProductTitle = x.product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchProductFileResponseDto>> SearchAsync(
        SearchProductFileRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var productFileSource = Context.ProductFile
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productFiles =
            from productFile in productFileSource
            where productFile.IsActive
            select productFile;

        if (request.ProductId.HasValue)
            productFiles = productFiles.Where(x => x.ProductId == request.ProductId.Value);

        if (request.IsMain.HasValue)
            productFiles = productFiles.Where(x => x.IsMain == request.IsMain.Value);

        if (!string.IsNullOrWhiteSpace(request.Title))
            productFiles = productFiles.Where(x => x.Translations.Any(t => t.Title.Contains(request.Title)));

        var result = await productFiles
            .Select(x => new SearchProductFileResponseDto
            {
                Id = x.Id,
                IsMain = x.IsMain,
                FileName = x.FileName,
                ProductId = x.ProductId,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
