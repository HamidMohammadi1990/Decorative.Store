using Edition.Application.Contracts.Orders;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Services;

public class OrderTrackingCodeGenerator(IOrderRepository orderRepository)
    : IOrderTrackingCodeGenerator
{
    private const int MaxAttempts = 20;

    public async Task<long> GenerateUniqueAsync(CancellationToken cancellationToken = default)
    {
        for (var attempt = 0; attempt < MaxAttempts; attempt++)
        {
            var trackingCode = GenerateRandomCode();
            if (!await orderRepository.ExistsByTrackingCodeAsync(trackingCode, cancellationToken))
                return trackingCode;
        }

        throw new InvalidOperationException("Unable to generate a unique order tracking code.");
    }

    private static long GenerateRandomCode()
    {
        var digitCount = Random.Shared.Next(9, 11);
        var min = (long)Math.Pow(10, digitCount - 1);
        var max = (long)Math.Pow(10, digitCount);
        return Random.Shared.NextInt64(min, max);
    }
}
