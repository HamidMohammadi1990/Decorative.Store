using Edition.Application.Contracts.Localization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.ProductPriceDeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductPriceDeliveryOptionRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<ProductPriceDeliveryOption>(context), IProductPriceDeliveryOptionRepository
{
    public async Task<PagedResult<GetAllProductPriceDeliveryOptionResponseDto>> GetAllAsync(GetAllProductPriceDeliveryOptionRequestDto request)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        var productPriceDeliveryOptionSource = Context.ProductPriceDeliveryOption
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query = from ProductPriceDeliveryOption in productPriceDeliveryOptionSource
                    join ProductPrice in Context.ProductPrice on ProductPriceDeliveryOption.ProductPriceId equals ProductPrice.Id
                    join DeliveryOption in Context.DeliveryOption on ProductPriceDeliveryOption.DeliveryOptionId equals DeliveryOption.Id
                    join Product in Context.Product on ProductPrice.ProductId equals Product.Id
                    join Company in Context.Company on ProductPrice.CompanyId equals Company.Id
                    select new { ProductPriceDeliveryOption, Company, Product, ProductPrice, DeliveryOption };

        query = query.ApplyQueryFilters(request);

        var result = await query
            .Select(x => new GetAllProductPriceDeliveryOptionResponseDto
            {
                Id = x.ProductPriceDeliveryOption.Id,
                Price = x.ProductPriceDeliveryOption.Price,
                CompanyId = x.Company.Id,
                ProductId = x.ProductPrice.ProductId,
                CompanyName = x.Company.Name,
                ProductTitle = x.Product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CreatedOnUtc = x.ProductPriceDeliveryOption.CreatedOnUtc,
                ProductPriceId = x.ProductPriceDeliveryOption.ProductPriceId,
                CooperationPrice = x.ProductPriceDeliveryOption.CooperationPrice,
                DeliveryOptionId = x.ProductPriceDeliveryOption.DeliveryOptionId,
                DeliveryOptionTitle = x.DeliveryOption.Title
            })
            .AsNoTracking()
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
