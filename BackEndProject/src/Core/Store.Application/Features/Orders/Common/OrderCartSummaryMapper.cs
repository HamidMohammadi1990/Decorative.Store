using Store.Domain.Dtos.Orders;

namespace Edition.Application.Features.Orders.Common;

public static class OrderCartSummaryMapper
{
    public static OrderCartSummaryResponse Map(CartModificationResult result)
        => new()
        {
            TotalPrice = result.TotalPrice,
            DiscountAmount = result.DiscountAmount,
            FinalPrice = result.FinalPrice,
            IsDiscountApplied = result.IsDiscountApplied,
            IsDiscountInvalidated = result.IsDiscountInvalidated,
            DiscountInvalidationMessage = result.InvalidationReason.HasValue
                ? DiscountFailureMapper.ToErrorModel(result.InvalidationReason.Value).Message
                : null
        };
}