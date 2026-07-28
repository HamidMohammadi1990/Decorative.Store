using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ContentPolicyRuleRepository
    (EditionDbContext context)
    : Repository<ContentPolicyRule>(context), IContentPolicyRuleRepository
{
    public async Task<ContentPolicyRule?> FindWithPolicyAsync(int id, CancellationToken cancellationToken = default)
        => await Context.ContentPolicyRule
            .Include(x => x.Policy)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<ContentPolicyRule>> GetAllAsync(GetAllContentPolicyRuleRequestDto request, CancellationToken cancellationToken = default)
    {
        var query = Context.ContentPolicyRule
            .Include(x => x.Policy)
            .AsNoTracking()
            .AsQueryable()
            .ApplyQueryFilters(request);

        return await query
            .OrderBy(x => x.PolicyId)
            .ThenBy(x => x.RuleGroup)
            .ThenBy(x => x.SortOrder)
            .ToPagedAsync(request.Pagination, cancellationToken: cancellationToken);
    }
}