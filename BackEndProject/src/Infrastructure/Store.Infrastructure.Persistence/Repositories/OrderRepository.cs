using Edition.Application.Contracts.Localization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Orders;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Repositories;

public class OrderRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Order>(context), IOrderRepository
{
    public async Task<PagedResult<GetAllOrderDto>> GetAllAsync(GetAllOrderRequestDto request)
    {
        var orderSource = Context.Order
            .ApplyContentPolicyFilter(request.ContentFilter);

        var orders =
            from order in orderSource
            join user in Context.User on order.UserId equals user.Id
            select new { order, user };

        orders = orders.ApplyQueryFilters(request);

        var result =
            await orders
                .Select(x => new GetAllOrderDto
                {
                    Id = x.order.Id,
                    Title = x.order.Title,
                    Status = x.order.Status,
                    UserId = x.order.UserId,
                    IsFinaly = x.order.IsFinaly,
                    VatPrice = x.order.VatPrice,
                    TotalPrice = x.order.TotalPrice,
                    FinalPrice = x.order.FinalPrice,
                    UserFirstName = x.user.FirstName!,
                    UserLastName = x.user.LastName!,
                    TrackingCode = x.order.TrackingCode,
                    CreatedOnUtc = x.order.CreatedOnUtc,
                    TotalCommissionPrice = x.order.TotalCommissionPrice
                })
                .AsNoTracking()
                .ToPagedAsync(request.Pagination);                

        return result;
    }

    public async Task<Order?> GetPendingOrderByUserIdAsync(int userId)
    {
        return await
            Context.Order
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.OrderItemProperties)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.OrderItemAttachments)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Status == OrderStatusType.Pending);
    }

    public Task<bool> HasPendingBankPaymentAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Context.BankTransaction
            .AnyAsync(
                x => x.FinancialDocument.OrderId == orderId &&
                     x.Status == TransactionStatusType.Pending,
                cancellationToken);
    }

    public Task<bool> HasFinancialDocumentsAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Context.FinancialDocument
            .AnyAsync(x => x.OrderId == orderId, cancellationToken);
    }

    public Task<bool> ExistsByTrackingCodeAsync(long trackingCode, CancellationToken cancellationToken = default)
    {
        return Context.Order
            .AnyAsync(x => x.TrackingCode == trackingCode, cancellationToken);
    }

    public async Task<Order?> GetByUserIdAsync(int userId, OrderStatusType status)
    {
        return await
            Context.Order
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.OrderItemProperties)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.OrderItemAttachments)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Status == status);
    }

    public void RemoveOrderItem(Order order, OrderItem orderItem)
    {
        if (orderItem.OrderItemProperties.Count > 0)
            Context.OrderItemProperty.RemoveRange(orderItem.OrderItemProperties);

        if (orderItem.OrderItemAttachments.Count > 0)
            Context.OrderItemAttachment.RemoveRange(orderItem.OrderItemAttachments);

        order.RemoveOrderItem(orderItem);
        Context.OrderItem.Remove(orderItem);
    }

    public async Task<List<GetStatusSummaryPropertiesDto>> GetUserOrderStatusSummaryAsync(int userId)
    {
        var result = await (from order in Context.Order
                            where order.UserId == userId && order.IsFinaly
                            group order by order.Status
                            into statusSummary
                            select new GetStatusSummaryPropertiesDto
                            {
                                Count = statusSummary.Count(),
                                Status = statusSummary.Key
                            })
                            .AsNoTracking()
                            .ToListAsync();
        return result;
    }

    public async Task<List<GetUserOrdersByStatusDto>> GetUserOrdersByStatusAsync(int userId, OrderStatusType status, PagedRequest pagination)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync();
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var defaultLanguageId = defaultLanguage.Id;

        var result = await (from order in Context.Order
                            join orderItem in Context.OrderItem
                            on order.Id equals orderItem.OrderId
                            join product in Context.Product
                            on orderItem.ProductId equals product.Id
                            where order.UserId == userId && order.Status == status && order.IsFinaly
                            select new GetUserOrdersByStatusDto
                            {
                                OrderId = order.Id,
                                Title = order.Title,
                                Status = order.Status,
                                OrderItemId = orderItem.Id,
                                FinalPrice = order.FinalPrice,
                                ItemQuantity = orderItem.Quantity,
                                TrackingCode = order.TrackingCode,
                                CreatedOnUtc = order.CreatedOnUtc,
                                ProductPrice = orderItem.ProductPrice,
                                ProductTitle = product.Translations
                                        .Where(t => t.LanguageId == languageId)
                                        .Select(t => t.Title)
                                        .FirstOrDefault()
                                    ?? product.Translations
                                        .Where(t => t.LanguageId == defaultLanguageId)
                                        .Select(t => t.Title)
                                        .FirstOrDefault()
                                    ?? string.Empty,
                                ProductImage = product.ProductFiles
                                                .Where(pf => pf.IsMain)
                                                .Select(pf => pf.FileName)
                                                .FirstOrDefault() ?? ""
                            })
                            .Pagination(pagination)
                            .AsNoTracking()
                            .ToListAsync();
        return result;
    }

    public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, int? userId, CancellationToken cancellationToken = default)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var defaultLanguageId = defaultLanguage.Id;

        var itemRows = await (
            from order in Context.Order
            where order.Id == orderId && (userId == null || order.UserId == userId)
            join item in Context.OrderItem on order.Id equals item.OrderId
            join product in Context.Product on item.ProductId equals product.Id
            join deliveryType in Context.DeliveryType on item.DeliveryTypeId equals deliveryType.Id into deliveryTypeJoin
            from deliveryType in deliveryTypeJoin.DefaultIfEmpty()
            join postType in Context.PostType on item.PostTypeId equals postType.Id into postTypeJoin
            from postType in postTypeJoin.DefaultIfEmpty()
            join address in Context.UserAddress on item.UserAddressId equals address.Id into addressJoin
            from address in addressJoin.DefaultIfEmpty()
            join city in Context.City on address.CityId equals city.Id into cityJoin
            from city in cityJoin.DefaultIfEmpty()
            select new
            {
                ItemId = item.Id,
                OrderId = order.Id,
                PostalCode = address != null ? address.PostalCode : null,
                product.ProductCode,
                PhoneNumber = address != null ? address.PhoneNumber : null,
                CityName = city != null ? city.Name : null,
                ProductId = product.Id,
                OrderTitle = order.Title,
                RecipientLastName = address != null ? address.RecipientLastName : null,
                RecipientFirstName = address != null ? address.RecipientFirstName : null,
                PostTitle = postType != null ? postType.Title : null,
                ProductSlug = product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                OrderStatus = order.Status,
                ProductTitle = product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                ItemQuantity = item.Quantity,
                AddressTitle = address != null ? address.Title : null,
                OrderIsFinaly = order.IsFinaly,
                AddressDetail = address != null ? address.Address : null,
                OrderItemStatusType = item.Status,
                ItemDescription = item.Description,
                DeliveryTitle = deliveryType != null ? deliveryType.Title : null,
                OrderFinalPrice = order.FinalPrice,
                OrderTotalPrice = order.TotalPrice,
                ItemCreatedOnUtc = item.CreatedOnUtc,
                ItemProductPrice = item.ProductPrice,
                OrderTrackingCode = order.TrackingCode,
                OrderCreatedOnUtc = order.CreatedOnUtc,
                ItemIsNeedToDesign = item.IsNeedToDesign,
                ItemEmergencyPhoneNumber = item.EmergencyPhoneNumber,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (itemRows.Count == 0)
            return null;

        var itemIds = itemRows.Select(row => row.ItemId).Distinct().ToList();

        var attachmentRows = await (
            from attachment in Context.OrderItemAttachment.AsNoTracking()
            where itemIds.Contains(attachment.OrderItemId)
            join productOrderItemAttachmentType in Context.ProductOrderItemAttachmentType
                on attachment.ProductOrderItemAttachmentTypeId equals productOrderItemAttachmentType.Id
            join orderItemAttachmentType in Context.OrderItemAttachmentType
                on productOrderItemAttachmentType.OrderItemAttachmentTypeId equals orderItemAttachmentType.Id
            select new
            {
                attachment.OrderItemId,
                attachment.FileName,
                TypeTitle = orderItemAttachmentType.Title,
            })
            .ToListAsync(cancellationToken);

        var attachmentsByItemId = attachmentRows
            .GroupBy(row => row.OrderItemId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(row => new OrderItemAttachmentDetailDto
                    {
                        FileName = row.FileName,
                        TypeTitle = row.TypeTitle,
                    })
                    .ToList());

        var propertyEntities = await Context.OrderItemProperty
            .AsNoTracking()
            .Where(property => itemIds.Contains(property.OrderItemId) && property.IsActive)
            .Include(property => property.Property!)
                .ThenInclude(p => p.Translations)
            .Include(property => property.PropertyItem!)
                .ThenInclude(item => item.Translations)
            .ToListAsync(cancellationToken);

        var propertiesByItemId = propertyEntities
            .GroupBy(property => property.OrderItemId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(property => MapOrderItemProperty(property, languageId, defaultLanguageId))
                    .ToList());

        var header = itemRows[0];

        return new OrderDetailDto
        {
            Id = header.OrderId,
            Title = header.OrderTitle,
            Status = header.OrderStatus,
            IsFinaly = header.OrderIsFinaly,
            TotalPrice = header.OrderTotalPrice,
            FinalPrice = header.OrderFinalPrice,
            CreatedOnUtc = header.OrderCreatedOnUtc,
            TrackingCode = header.OrderTrackingCode,
            Items = [.. itemRows.Select(row => new OrderDetailItemDto
            {
                Id = row.ItemId,
                Quantity = row.ItemQuantity,
                PostTypeTitle = row.PostTitle,
                Status = row.OrderItemStatusType,
                Description = row.ItemDescription,
                ProductPrice = row.ItemProductPrice,
                CreatedOnUtc = row.ItemCreatedOnUtc,
                DeliveryTypeTitle = row.DeliveryTitle,
                IsNeedToDesign = row.ItemIsNeedToDesign,
                EmergencyPhoneNumber = row.ItemEmergencyPhoneNumber,
                Product = new OrderItemProductSummaryDto
                {
                    Id = row.ProductId,
                    Title = row.ProductTitle,
                    Slug = row.ProductSlug,
                    ProductCode = row.ProductCode,
                },
                UserAddress = new OrderItemUserAddressDto
                {
                    Title = row.AddressTitle ?? string.Empty,
                    CityTitle = row.CityName ?? string.Empty,
                    Address = row.AddressDetail ?? string.Empty,
                    PostalCode = row.PostalCode,
                    PhoneNumber = row.PhoneNumber ?? string.Empty,
                    RecipientLastName = row.RecipientLastName,
                    RecipientFirstName = row.RecipientFirstName,
                },
                Attachments = attachmentsByItemId.GetValueOrDefault(row.ItemId) ?? [],
                Properties = propertiesByItemId.GetValueOrDefault(row.ItemId) ?? [],
            })],
        };
    }

    private static OrderItemPropertyDetailDto MapOrderItemProperty(
        OrderItemProperty property,
        int languageId,
        int defaultLanguageId)
    {
        var propertyTitle = ResolveTranslationTitle(property.Property?.Translations, languageId, defaultLanguageId);
        var propertyItemTitle = ResolveTranslationTitle(property.PropertyItem?.Translations, languageId, defaultLanguageId);

        var detail = new OrderItemPropertyDetailDto
        {
            Title = propertyTitle,
            Price = property.PropertyPrice,
            PropertyType = property.PropertyType,
            ItemTitle = propertyItemTitle,
            ItemPrice = property.PropertyItemPrice,
        };

        return property switch
        {
            TextOrderItemProperty text => detail with { Value = text.Value },
            NumericOrderItemProperty numeric => detail with { Quantity = numeric.Quantity },
            DimensionsOrderItemProperty dimensions => detail with
            {
                Width = dimensions.Width,
                Height = dimensions.Height,
            },
            BooleanOrderItemProperty boolean => detail with { IsSelected = boolean.IsSelected },
            _ => detail,
        };
    }

    private static string ResolveTranslationTitle(
        IEnumerable<PropertyTranslation>? translations,
        int languageId,
        int defaultLanguageId)
    {
        if (translations == null)
            return string.Empty;

        return translations
            .Where(translation => translation.LanguageId == languageId)
            .Select(translation => translation.Title)
            .FirstOrDefault()
            ?? translations
                .Where(translation => translation.LanguageId == defaultLanguageId)
                .Select(translation => translation.Title)
                .FirstOrDefault()
            ?? string.Empty;
    }

    private static string ResolveTranslationTitle(
        IEnumerable<PropertyItemTranslation>? translations,
        int languageId,
        int defaultLanguageId)
    {
        if (translations == null)
            return string.Empty;

        return translations
            .Where(translation => translation.LanguageId == languageId)
            .Select(translation => translation.Title)
            .FirstOrDefault()
            ?? translations
                .Where(translation => translation.LanguageId == defaultLanguageId)
                .Select(translation => translation.Title)
                .FirstOrDefault()
            ?? string.Empty;
    }
}