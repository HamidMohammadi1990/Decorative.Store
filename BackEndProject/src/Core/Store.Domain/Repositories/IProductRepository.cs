using System.Linq.Expressions;
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
    Task<Product?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Product>> GetAllAsync(GetAllProductRequestDto request, CancellationToken cancellationToken = default);
}