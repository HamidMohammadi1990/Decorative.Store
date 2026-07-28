using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IContentPolicyRuleRepository
{
    void Add(ContentPolicyRule rule);
    void Remove(ContentPolicyRule rule);
    ValueTask<ContentPolicyRule?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ContentPolicyRule?> FindWithPolicyAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<ContentPolicyRule>> GetAllAsync(GetAllContentPolicyRuleRequestDto request, CancellationToken cancellationToken = default);
}