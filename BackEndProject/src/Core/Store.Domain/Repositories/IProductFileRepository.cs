using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFiles;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductFileRepository
{
    void Add(ProductFile productFile);
    void Remove(ProductFile productFile);
    Task<ProductFile?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<ProductFile?> FindAsync(int productFileId, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProductFileResponseDto>> GetAllAsync(GetAllProductFileRequestDto request);
    Task<PagedResult<SearchProductFileResponseDto>> SearchAsync(SearchProductFileRequestDto request);
}