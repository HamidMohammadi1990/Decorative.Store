using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Orders;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Repositories;

public interface IOrderRepository
{
    void Add(Order order);
    void Remove(Order order);
    void RemoveOrderItem(Order order, OrderItem orderItem);
    Task<Order?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllOrderDto>> GetAllAsync(GetAllOrderRequestDto request);
    Task<Order?> GetPendingOrderByUserIdAsync(int userId);
    Task<Order?> GetByUserIdAsync(int userId, OrderStatusType status);
    Task<bool> HasPendingBankPaymentAsync(int orderId, CancellationToken cancellationToken = default);
    Task<bool> HasFinancialDocumentsAsync(int orderId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTrackingCodeAsync(long trackingCode, CancellationToken cancellationToken = default);
    Task<List<GetStatusSummaryPropertiesDto>> GetUserOrderStatusSummaryAsync(int userId);
    Task<OrderDetailDto?> GetOrderDetailAsync(int orderId, int userId, CancellationToken cancellationToken = default);
    Task<List<GetUserOrdersByStatusDto>> GetUserOrdersByStatusAsync(int userId, OrderStatusType status, PagedRequest pagination);
}