using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPageTypeGuideRepository
{
    Task<List<PageTypeGuide>> GetAllAsync(CancellationToken cancellationToken = default);
}
