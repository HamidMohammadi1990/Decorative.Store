using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductDescriptions;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductDescriptionRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<ProductDescription>(context), IProductDescriptionRepository
{
    public async Task<PagedResult<GetAllProductDescriptionResponseDto>> GetAllAsync(GetAllProductDescriptionRequestDto request)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        var descriptions = Context
               .ProductDescription
               .ApplyContentPolicyFilter(request.ContentFilter)
               .Include(x => x.Product)
               .ApplyQueryFilters(request);

        var result =
            await descriptions
            .AsNoTracking()
            .Select(x => new GetAllProductDescriptionResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                LanguageId = x.LanguageId,
                Description = x.Description,
                ProductTitle = x.Product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchProductDescriptionResponseDto>> SearchAsync(SearchProductDescriptionRequestDto request)
    {
        var descriptions = Context
               .ProductDescription
               .ApplyContentPolicyFilter(request.ContentFilter)
               .ApplyQueryFilters(request);

        var result =
            await descriptions
            .AsNoTracking()
            .Select(x => new SearchProductDescriptionResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                LanguageId = x.LanguageId,
                Description = x.Description
            })
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
