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
    Task<CatalogProductDto?> GetCatalogProductBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<CatalogListingProductDto>> GetRelatedCatalogProductsBySlugAsync(string slug, int limit, CancellationToken cancellationToken = default);
    Task<List<CatalogListingProductDto>> GetNewestCatalogProductsAsync(int limit, CancellationToken cancellationToken = default);
    Task<List<CatalogListingProductDto>> GetNewArrivalsCatalogProductsAsync(int? limit = null, CancellationToken cancellationToken = default);
    Task<List<CatalogListingProductDto>> GetInStockCatalogProductsAsync(int? limit = null, CancellationToken cancellationToken = default);
    Task<List<CatalogListingProductDto>> GetBestSellingCatalogProductsAsync(int? limit = null, CancellationToken cancellationToken = default);
    Task<CatalogSearchDto> SearchCatalogAsync(string query, int limit, CancellationToken cancellationToken = default);
}
