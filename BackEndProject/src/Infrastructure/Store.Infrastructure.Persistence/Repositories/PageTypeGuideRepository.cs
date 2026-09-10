using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class PageTypeGuideRepository(EditionDbContext context) : IPageTypeGuideRepository
{
    public Task<List<PageTypeGuide>> GetAllAsync(CancellationToken cancellationToken = default)
        => context.PageTypeGuide
            .AsNoTracking()
            .OrderBy(x => x.Type)
            .ToListAsync(cancellationToken);
}
