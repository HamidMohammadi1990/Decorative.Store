using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductDescriptions;
using Store.Domain.Entities;

namespace Store.Domain.Repositories
{
    public interface IProductDescriptionRepository
    {
        ValueTask<ProductDescription?> FindAsync(int id, CancellationToken cancellationToken = default);
        void Add(ProductDescription productDescription);
        void Remove(ProductDescription productDescription);
        Task<ProductDescription?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<GetAllProductDescriptionResponseDto>> GetAllAsync(GetAllProductDescriptionRequestDto request);
        Task<PagedResult<SearchProductDescriptionResponseDto>> SearchAsync(SearchProductDescriptionRequestDto request);
    }
}