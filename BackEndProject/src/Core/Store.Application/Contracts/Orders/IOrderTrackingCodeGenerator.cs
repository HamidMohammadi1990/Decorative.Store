namespace Edition.Application.Contracts.Orders;

public interface IOrderTrackingCodeGenerator
{
    Task<long> GenerateUniqueAsync(CancellationToken cancellationToken = default);
}
