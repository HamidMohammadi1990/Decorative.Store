using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Repositories;

public interface IContentPolicyRepository
{
    void Add(ContentPolicy policy);
    void Remove(ContentPolicy policy);
    ValueTask<ContentPolicy?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ContentPolicy?> FindWithRulesAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<ContentPolicy>> GetAllAsync(GetAllContentPolicyRequestDto request, CancellationToken cancellationToken = default);
    Task<List<UserRolePolicyDto>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<int>> GetCompanyIdsByOwnerUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserContentPolicyContextData?> GetUserContentPolicyContextAsync(int userId, CancellationToken cancellationToken = default);
    Task<ContentPolicyResolutionResult> ResolveActivePoliciesAsync(
        string entityType,
        ContentPolicyQueryAction queryAction,
        int userId,
        IReadOnlyList<int> roleIds,
        CancellationToken cancellationToken = default);
    Task<ContentPolicyActivePolicySets> GetActivePolicySetsAsync(
        GetContentPolicyPolicySetsRequestDto request,
        CancellationToken cancellationToken = default);
}
