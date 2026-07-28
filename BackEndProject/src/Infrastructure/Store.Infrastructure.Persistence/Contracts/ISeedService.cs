using Store.Domain.Dtos.Others;

namespace Store.Infrastructure.Persistence.Contracts;

public interface ISeedService
{
    Task SeedDataAsync(List<DynamicPermission> permissions);
}