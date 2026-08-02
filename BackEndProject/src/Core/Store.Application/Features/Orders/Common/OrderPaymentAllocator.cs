using Store.Domain.Entities;

namespace Edition.Application.Features.Orders.Common;

public static class OrderPaymentAllocator
{
    public static OrderPaymentSlice? Allocate(Order order)
    {
        var grossAmount = order.OrderItems.Sum(item => item.GetSumPrices() * item.Quantity);
        if (grossAmount <= 0)
            return null;

        var netAmount = Math.Max(0m, grossAmount - order.DiscountAmount);

        return new OrderPaymentSlice(
            grossAmount,
            order.DiscountAmount,
            netAmount,
            order.VatPrice,
            order.FinalPrice);
    }
}
