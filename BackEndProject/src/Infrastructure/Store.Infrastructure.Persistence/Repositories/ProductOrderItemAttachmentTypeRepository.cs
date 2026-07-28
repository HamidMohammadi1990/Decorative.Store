using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Products;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductOrderItemAttachmentTypes;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductOrderItemAttachmentTypeRepository
    (EditionDbContext context)
    : Repository<ProductOrderItemAttachmentType>(context), IProductOrderItemAttachmentTypeRepository
{
    public async Task<PagedResult<ProductOrderItemAttachmentType>> GetAllAsync(
        GetAllProductOrderItemAttachmentTypeRequestDto request)
    {
        var source = Context.ProductOrderItemAttachmentType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<ProductOrderItemAttachmentType>> SearchAsync(
        SearchProductOrderItemAttachmentTypeRequestDto request)
    {
        var source = Context.ProductOrderItemAttachmentType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<List<ProductAttachmentTypeRestrictionDto>> GetRestrictionByProductIdAsync(int productId)
    {
        var restrictions =
            await (from attachmentType in Context.ProductOrderItemAttachmentType
                   join attachmentTypeRestriction in Context.OrderItemAttachmentTypeRestriction
                   on new { attachmentType.Id, attachmentType.ProductId } equals
                      new { Id = attachmentTypeRestriction.ProductOrderItemAttachmentTypeId, ProductId = productId }
                   select new ProductAttachmentTypeRestrictionDto
                   {
                       IsRequired = attachmentTypeRestriction.IsRequired,
                       MinWidth = attachmentTypeRestriction.MinWidth,
                       MaxWidth = attachmentTypeRestriction.MaxWidth,
                       MinHeight = attachmentTypeRestriction.MinHeight,
                       MaxHeight = attachmentTypeRestriction.MaxHeight,
                       MaxFileSizeInBytes = attachmentTypeRestriction.MaxFileSizeInBytes,
                       MinVerticalResolution = attachmentTypeRestriction.MinVerticalResolution,
                       MaxVerticalResolution = attachmentTypeRestriction.MaxVerticalResolution,
                       MinHorizontalResolution = attachmentTypeRestriction.MinHorizontalResolution,
                       MaxHorizontalResolution = attachmentTypeRestriction.MaxHorizontalResolution,
                       ProductOrderItemAttachmentTypeId = attachmentTypeRestriction.ProductOrderItemAttachmentTypeId
                   })
                   .AsNoTracking()
                   .ToListAsync();

        return restrictions;
    }

    public async Task<List<ProductOrderAttachmentDto>> GetCheckoutProductAttachmentsAsync(int productId)
    {
        var attachments =
            await (from itemAttachmentType in Context.ProductOrderItemAttachmentType
                   join attachmentType in Context.OrderItemAttachmentType
                   on itemAttachmentType.OrderItemAttachmentTypeId equals attachmentType.Id
                   join attachmentTypeRestriction in Context.OrderItemAttachmentTypeRestriction
                   on new { itemAttachmentType.Id, itemAttachmentType.ProductId } equals
                      new { Id = attachmentTypeRestriction.ProductOrderItemAttachmentTypeId, ProductId = productId }
                   orderby itemAttachmentType.Priority descending
                   select new ProductOrderAttachmentDto
                   {
                       Title = attachmentType.Title,
                       IsRequired = attachmentTypeRestriction.IsRequired,
                       Description = itemAttachmentType.Description,
                       MinWidth = attachmentTypeRestriction.MinWidth,
                       MaxWidth = attachmentTypeRestriction.MaxWidth,
                       MinHeight = attachmentTypeRestriction.MinHeight,
                       MaxHeight = attachmentTypeRestriction.MaxHeight,
                       MaxFileSizeInBytes = attachmentTypeRestriction.MaxFileSizeInBytes,
                       MinVerticalResolution = attachmentTypeRestriction.MinVerticalResolution,
                       MaxVerticalResolution = attachmentTypeRestriction.MaxVerticalResolution,
                       MinHorizontalResolution = attachmentTypeRestriction.MinHorizontalResolution,
                       MaxHorizontalResolution = attachmentTypeRestriction.MaxHorizontalResolution,
                       ProductOrderItemAttachmentTypeId = attachmentTypeRestriction.ProductOrderItemAttachmentTypeId
                   })
                   .AsNoTracking()
                   .ToListAsync();

        return attachments;
    }
}