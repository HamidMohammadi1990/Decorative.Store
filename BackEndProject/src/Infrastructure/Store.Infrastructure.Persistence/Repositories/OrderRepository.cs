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
    (EditionDbContext context)
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
            .ThenInclude(x => x.Product)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Status == status);
    }

    public async Task<List<GetStatusSummaryPropertiesDto>> GetUserOrderStatusSummaryAsync(int userId)
    {
        var result = await (from order in Context.Order
                            where order.UserId == userId
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
        var result = await (from order in Context.Order
                            join orderItem in Context.OrderItem
                            on order.Id equals orderItem.OrderId
                            join product in Context.Product
                            on orderItem.ProductId equals product.Id
                            where order.UserId == userId && order.Status == status
                            select new GetUserOrdersByStatusDto
                            {
                                OrderId = order.Id,
                                Title = order.Title,
                                OrderItemId = orderItem.Id,
                                FinalPrice = order.FinalPrice,
                                ItemQuantity = orderItem.Quantity,
                                TrackingCode = order.TrackingCode,
                                CreatedOnUtc = orderItem.CreatedOnUtc,
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

    public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, int userId, CancellationToken cancellationToken = default)
    {
        var query =
            from order in Context.Order
            where order.Id == orderId && order.UserId == userId
            join item in Context.OrderItem on order.Id equals item.OrderId
            join product in Context.Product on item.ProductId equals product.Id
            join company in Context.Company on item.CompanyId equals company.Id
            join deliveryType in Context.DeliveryType on item.DeliveryTypeId equals deliveryType.Id
            join postType in Context.PostType on item.PostTypeId equals postType.Id into postTypeJoin
            from postType in postTypeJoin.DefaultIfEmpty()
            join address in Context.UserAddress on item.UserAddressId equals address.Id into addressJoin
            from address in addressJoin.DefaultIfEmpty()
            join city in Context.City on address.CityId equals city.Id into cityJoin
            from city in cityJoin.DefaultIfEmpty()
            join orderItemAttachment in Context.OrderItemAttachment on item.Id equals orderItemAttachment.OrderItemId into orderItemAttachmentJoin
            from orderItemAttachment in orderItemAttachmentJoin.DefaultIfEmpty()
            join productOrderItemAttachmentType in Context.ProductOrderItemAttachmentType
                on orderItemAttachment.ProductOrderItemAttachmentTypeId equals productOrderItemAttachmentType.Id into productOrderItemAttachmentTypeJoin
            from productOrderItemAttachmentType in productOrderItemAttachmentTypeJoin.DefaultIfEmpty()
            join orderItemAttachmentType in Context.OrderItemAttachmentType
                on productOrderItemAttachmentType.OrderItemAttachmentTypeId equals orderItemAttachmentType.Id into orderItemAttachmentTypeJoin
            from orderItemAttachmentType in orderItemAttachmentTypeJoin.DefaultIfEmpty()
            join orderItemProperty in Context.OrderItemProperty on item.Id equals orderItemProperty.OrderItemId into orderItemPropertyJoin
            from orderItemProperty in orderItemPropertyJoin.DefaultIfEmpty()
            join propertyItem in Context.PropertyItem on orderItemProperty.PropertyItemId equals propertyItem.Id into propertyItemJoin
            from propertyItem in propertyItemJoin.DefaultIfEmpty()
            join property in Context.Property on propertyItem.PropertyId equals property.Id into propertyJoin
            from property in propertyJoin.DefaultIfEmpty()
            select new
            {
                ItemId = item.Id,
                OrderId = order.Id,
                address.PostalCode,
                product.ProductCode,
                address.PhoneNumber,
                CityName = city.Name,
                AddressId = address.Id,
                ProductId = product.Id,
                CompanyId = company.Id,
                OrderTitle = order.Title,
                address.RecipientLastName,
                address.RecipientFirstName,
                PostTitle = postType.Title,
                CompanyCode = company.Code,
                ProductSlug = product.Slug,
                CompanyName = company.Name,
                OrderStatus = order.Status,
                ProductTitle = product.Title,
                ItemQuantity = item.Quantity,
                AddressTitle = address.Title,
                PropertyTitle = property.Title,
                orderItemProperty.PropertyType,
                OrderIsFinaly = order.IsFinaly,
                orderItemProperty.PropertyPrice,
                AddressDetail = address.Address,
                OrderItemStatusType = item.Status,
                PropertyId = orderItemProperty.Id,
                ItemDescription = item.Description,
                DeliveryTitle = deliveryType.Title,
                OrderFinalPrice = order.FinalPrice,
                OrderTotalPrice = order.TotalPrice,
                orderItemProperty.PropertyItemPrice,
                ItemCreatedOnUtc = item.CreatedOnUtc,
                ItemProductPrice = item.ProductPrice,
                OrderTrackingCode = order.TrackingCode,
                OrderCreatedOnUtc = order.CreatedOnUtc,
                PropertyItemTitle = propertyItem.Title,
                CompanyPhoneNumber = company.PhoneNumber,
                AttachmentFileName = orderItemAttachment.FileName,
                ItemIsNeedToDesign = item.IsNeedToDesign,
                AttachmentTypeTitle = orderItemAttachmentType.Title,
                ItemEmergencyPhoneNumber = item.EmergencyPhoneNumber,
                ((TextOrderItemProperty)orderItemProperty).Value,
                ((NumericOrderItemProperty)orderItemProperty).Quantity,
                ((DimensionsOrderItemProperty)orderItemProperty).Width,
                ((DimensionsOrderItemProperty)orderItemProperty).Height,
                ((BooleanOrderItemProperty)orderItemProperty).IsSelected,
            };

        var orderDetail = await query.AsNoTracking().ToListAsync(cancellationToken);

        var result = orderDetail
            .Select(order => new OrderDetailDto
            {
                Id = order.OrderId,
                Title = order.OrderTitle,
                Status = order.OrderStatus,
                IsFinaly = order.OrderIsFinaly,
                TotalPrice = order.OrderTotalPrice,
                FinalPrice = order.OrderFinalPrice,
                CreatedOnUtc = order.OrderCreatedOnUtc,
                TrackingCode = order.OrderTrackingCode,
                Items = [.. orderDetail
                    .GroupBy(i => i.ItemId)
                    .Select(itemGroup =>
                    {
                        var orderItem = itemGroup.First();
                        var items = new OrderDetailItemDto
                        {
                            Id = itemGroup.Key,
                            Quantity = orderItem.ItemQuantity,
                            PostTypeTitle = orderItem.PostTitle,
                            Status = orderItem.OrderItemStatusType,
                            Description = orderItem.ItemDescription,
                            ProductPrice = orderItem.ItemProductPrice,
                            CreatedOnUtc = orderItem.ItemCreatedOnUtc,
                            DeliveryTypeTitle = orderItem.DeliveryTitle,
                            IsNeedToDesign = orderItem.ItemIsNeedToDesign,
                            EmergencyPhoneNumber = orderItem.ItemEmergencyPhoneNumber,
                            Product = new OrderItemProductSummaryDto
                            {
                                Id = orderItem.ProductId,
                                Title = orderItem.ProductTitle,
                                Slug = orderItem.ProductSlug,
                                ProductCode = orderItem.ProductCode
                            },
                            Company = new OrderItemCompanySummaryDto
                            {
                                Id = orderItem.CompanyId,
                                Name = orderItem.CompanyName,
                                Code = orderItem.CompanyCode,
                                PhoneNumber = orderItem.CompanyPhoneNumber
                            },
                            UserAddress = new OrderItemUserAddressDto
                            {
                                Title = orderItem.AddressTitle,
                                CityTitle = orderItem.CityName,
                                Address = orderItem.AddressDetail,
                                PostalCode = orderItem.PostalCode,
                                PhoneNumber = orderItem.PhoneNumber,
                                RecipientLastName = orderItem.RecipientLastName,
                                RecipientFirstName = orderItem.RecipientFirstName,
                            },
                            Attachments = [.. itemGroup
                            .Where(a => a.AttachmentFileName != null)
                            .Select(a => new OrderItemAttachmentDetailDto
                            {
                                FileName = a.AttachmentFileName!,
                                TypeTitle = a.AttachmentTypeTitle!
                            })],
                            Properties = [.. itemGroup
                            .Where(p => p.PropertyId != null)
                            .Select(p => new OrderItemPropertyDetailDto
                            {
                                Width = p.Width,
                                Height = p.Height,
                                Quantity = p.Quantity,
                                Title = p.PropertyTitle,
                                Price = p.PropertyPrice,
                                Value = p.Value,
                                IsSelected = p.IsSelected,
                                PropertyType = p.PropertyType,
                                ItemTitle = p.PropertyItemTitle,
                                ItemPrice = p.PropertyItemPrice,
                            })]
                        };

                        return items;
                    })]
            }).FirstOrDefault();

        return result;
    }
}