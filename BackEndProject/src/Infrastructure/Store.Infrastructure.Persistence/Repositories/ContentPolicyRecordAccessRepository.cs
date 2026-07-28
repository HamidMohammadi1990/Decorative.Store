using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ContentPolicyRecordAccessRepository
    (EditionDbContext context)
    : Repository<ContentPolicyRecordAccess>(context), IContentPolicyRecordAccessRepository
{
    public void RemoveRange(IEnumerable<ContentPolicyRecordAccess> recordAccesses)
        => Context.ContentPolicyRecordAccess.RemoveRange(recordAccesses);

    public async Task<ContentPolicyRecordAccess?> FindWithPolicyAsync(int id, CancellationToken cancellationToken = default)
        => await Context.ContentPolicyRecordAccess
            .Include(x => x.Policy)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<ContentPolicyRecordAccess>> GetByPolicyIdAsync(int policyId, CancellationToken cancellationToken = default)
        => Context.ContentPolicyRecordAccess
            .Where(x => x.PolicyId == policyId)
            .OrderBy(x => x.EntityId)
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(int policyId, int entityId, CancellationToken cancellationToken = default)
        => Context.ContentPolicyRecordAccess
            .AnyAsync(x => x.PolicyId == policyId && x.EntityId == entityId, cancellationToken);

    public async Task<PagedResult<ContentPolicyRecordAccess>> GetAllAsync(
        GetAllContentPolicyRecordAccessRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var query = Context.ContentPolicyRecordAccess
            .Include(x => x.Policy)
            .AsNoTracking()
            .AsQueryable()
            .ApplyQueryFilters(request);

        return await query
            .OrderBy(x => x.PolicyId)
            .ThenBy(x => x.EntityId)
            .ToPagedAsync(request.Pagination, cancellationToken: cancellationToken);
    }
}