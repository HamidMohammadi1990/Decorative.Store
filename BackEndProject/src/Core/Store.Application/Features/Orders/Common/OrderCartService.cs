using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Dtos.Orders;

namespace Edition.Application.Features.Orders.Common;

public static class OrderCartService
{
    public static async Task<CartModificationResult> RefreshCartAsync(
        Order order,
        IDiscountRepository discountRepository,
        bool isCooperation,
        CancellationToken cancellationToken = default)
    {
        Discount? appliedDiscount = null;
        if (order.AppliedDiscountId.HasValue)
            appliedDiscount = await discountRepository.GetByIdAsync(order.AppliedDiscountId.Value);

        return order.RefreshAfterCartChange(appliedDiscount, isCooperation);
    }
}