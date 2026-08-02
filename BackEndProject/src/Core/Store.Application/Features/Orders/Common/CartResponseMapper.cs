using Edition.Application.Features.Orders.Commands;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Orders.Common;

internal static class CartResponseMapper
{
    public static async Task<GetCartResponse> MapAsync(
        IProductRepository productRepository,
        Order order,
        CartModificationResult cartResult,
        CancellationToken cancellationToken)
    {
        var items = new List<CartItemResponse>();

        foreach (var orderItem in order.OrderItems)
        {
            var summary = await productRepository.GetProductSummaryByIdAsync(orderItem.ProductId);
            var image = summary?.Images.FirstOrDefault();

            items.Add(new CartItemResponse
            {
                OrderItemId = orderItem.Id,
                ProductId = orderItem.ProductId,
                Slug = summary?.Slug ?? string.Empty,
                Title = summary?.Title ?? string.Empty,
                ImageUrl = image?.Url,
                ImageAlt = image?.Title,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.ProductPrice,
                CurrencyCode = "IRT"
            });
        }

        return new GetCartResponse
        {
            OrderId = order.Id,
            TrackingCode = order.TrackingCode,
            Summary = OrderCartSummaryMapper.Map(cartResult),
            Items = items
        };
    }
}
