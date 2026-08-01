using Store.Domain.Dtos.Others;

namespace Store.Infrastructure.Persistence.Contracts;

public interface ISeedService
{
    Task SeedCatalogAsync(CancellationToken cancellationToken = default);
    Task SeedDataAsync(List<DynamicPermission> permissions, CancellationToken cancellationToken = default);
}