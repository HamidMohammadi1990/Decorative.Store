using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Services.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ContentPolicies.Commands;

public class UpdateContentPolicyHandler
    (
        IUnitOfWork uow,
        IContentPolicyRepository contentPolicyRepository,
        ContentPolicyRuleValidator ruleValidator,
        IContentPolicyCache contentPolicyCache)
    : IRequestHandler<UpdateContentPolicyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateContentPolicyRequest request, CancellationToken cancellationToken)
    {
        var policy = await contentPolicyRepository.FindWithRulesAsync(request.Id, cancellationToken);
        if (policy is null)
            return OperationResult.Fail();

        var ruleDtos = request.Rules
            .Select(x => new ContentPolicyRuleDto(x.FieldPath, x.Operator, x.ValueType, x.Value, x.SortOrder, x.RuleGroup))
            .ToList();

        var errors = ruleValidator.ValidateRules(policy.EntityType, ruleDtos);
        if (errors.Count > 0)
            return OperationResult.Fail();

        policy.UpdateScope(request.RoleId, request.UserId, request.MergeMode);
        policy.Update(request.Name, request.Effect, request.IsActive, request.Priority, request.QueryAction);
        policy.ReplaceRules(ruleDtos
            .Select(x => ContentPolicyRule.Create(x.FieldPath, x.Operator, x.ValueType, x.Value, x.SortOrder, x.RuleGroup))
            .ToArray());

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return OperationResult.Fail();

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return OperationResult.Success();
    }
}
