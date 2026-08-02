using System.Linq.Expressions;
using Store.Domain.Dtos.Catalog;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductRepository
{
    Task<ProductSummaryDto?> GetProductSummaryByIdAsync(int id);
    void Add(Product product);
    Task<bool> AnyAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<Product?> FindAsync(int productid, CancellationToken cancellationToken = default);
    Task<Product?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<Product?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Product?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsProductCodeAsync(string productCode, int? excludeProductId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeProductId = null,
        CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProductResponseDto>> GetAllAsync(GetAllProductRequestDto request, CancellationToken cancellationToken = default);
    Task<CatalogListingDto> GetCatalogListingByPathAsync(string catalogPath, CancellationToken cancellationToken = default);
}
