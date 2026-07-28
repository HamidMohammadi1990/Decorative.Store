using System.Linq.Expressions;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductOrderItemAttachmentTypes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductOrderItemAttachmentTypeRepository
{
    void Add(ProductOrderItemAttachmentType productOrderItemAttachmentType);
    void Remove(ProductOrderItemAttachmentType productOrderItemAttachmentType);
    ValueTask<ProductOrderItemAttachmentType?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductOrderItemAttachmentType?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<ProductOrderItemAttachmentType, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductOrderItemAttachmentType>> GetAllAsync(GetAllProductOrderItemAttachmentTypeRequestDto request);
    Task<PagedResult<ProductOrderItemAttachmentType>> SearchAsync(SearchProductOrderItemAttachmentTypeRequestDto request);
    Task<List<ProductAttachmentTypeRestrictionDto>> GetRestrictionByProductIdAsync(int productId);
    Task<List<ProductOrderAttachmentDto>> GetCheckoutProductAttachmentsAsync(int productId);
}