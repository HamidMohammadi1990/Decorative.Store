using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Repositories;

public class ContentPolicyRepository
    (EditionDbContext context)
    : Repository<ContentPolicy>(context), IContentPolicyRepository
{
    public async Task<ContentPolicy?> FindWithRulesAsync(int id, CancellationToken cancellationToken = default)
        => await Context.ContentPolicy
            .Include(x => x.Rules)
            .Include(x => x.RecordAccesses)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<ContentPolicy>> GetAllAsync(GetAllContentPolicyRequestDto request, CancellationToken cancellationToken = default)
    {
        var query = Context.ContentPolicy
            .Include(x => x.Rules)
            .AsNoTracking()
            .AsQueryable()
            .ApplyQueryFilters(request);

        return await query
            .OrderBy(x => x.Priority)
            .ToPagedAsync(request.Pagination, cancellationToken: cancellationToken);
    }

    public async Task<List<UserRolePolicyDto>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.UserRole
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new UserRolePolicyDto(
                x.Role.Id,
                x.Role.Title,
                x.Role.BypassContentPolicy,
                x.Role.RequireContentPolicy))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<int>> GetCompanyIdsByOwnerUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Company
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.IsActive)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserContentPolicyContextData?> GetUserContentPolicyContextAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userExists = await Context.User
            .AsNoTracking()
            .AnyAsync(x => x.Id == userId, cancellationToken);
        if (!userExists)
            return null;

        var roles = await GetUserRolesAsync(userId, cancellationToken);
        var companyIds = await GetCompanyIdsByOwnerUserIdAsync(userId, cancellationToken);
        return new UserContentPolicyContextData(roles, companyIds);
    }

    public async Task<ContentPolicyResolutionResult> ResolveActivePoliciesAsync(
        string entityType,
        ContentPolicyQueryAction queryAction,
        int userId,
        IReadOnlyList<int> roleIds,
        CancellationToken cancellationToken = default)
    {
        var sets = await GetActivePolicySetsAsync(
            new GetContentPolicyPolicySetsRequestDto
            {
                EntityType = entityType,
                QueryAction = queryAction,
                UserId = userId,
                RoleIds = roleIds
            },
            cancellationToken);
        return ContentPolicyPolicyResolver.Resolve(sets.UserPolicies, sets.RolePolicies);
    }

    public async Task<ContentPolicyActivePolicySets> GetActivePolicySetsAsync(
        GetContentPolicyPolicySetsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = BuildActivePolicyQuery(request.EntityType, request.QueryAction);

        var userPolicies = await baseQuery
            .Include(x => x.Rules)
            .Include(x => x.RecordAccesses)
            .Where(x => x.UserId == request.UserId)
            .Select(x => MapPolicy(x))
            .ToListAsync(cancellationToken);

        var rolePolicies = request.RoleIds.Count == 0
            ? []
            : await baseQuery
                .Include(x => x.Rules)
                .Include(x => x.RecordAccesses)
                .Where(x => x.UserId == null && x.RoleId != null && request.RoleIds.Contains(x.RoleId.Value))
                .Select(x => MapPolicy(x))
                .ToListAsync(cancellationToken);

        return new ContentPolicyActivePolicySets(userPolicies, rolePolicies);
    }

    private IQueryable<ContentPolicy> BuildActivePolicyQuery(
        string entityType,
        ContentPolicyQueryAction queryAction)
    {
        return Context.ContentPolicy
            .AsNoTracking()
            .Where(x => x.IsActive && x.EntityType == entityType)
            .Where(x => ContentPolicyQueryActionExtensions.Matches(x.QueryAction, queryAction));
    }

    private static ContentPolicyWithRulesDto MapPolicy(ContentPolicy x)
        => new(
            x.Id,
            x.RoleId,
            x.UserId,
            x.MergeMode,
            x.EntityType,
            x.QueryAction,
            x.Name,
            x.Effect,
            x.Priority,
            x.Rules
                .OrderBy(r => r.SortOrder)
                .Select(r => new ContentPolicyRuleDto(
                    r.FieldPath,
                    r.Operator,
                    r.ValueType,
                    r.Value,
                    r.SortOrder,
                    r.RuleGroup))
                .ToList(),
            x.RecordAccesses
                .OrderBy(r => r.EntityId)
                .Select(r => r.EntityId)
                .ToList());
}
