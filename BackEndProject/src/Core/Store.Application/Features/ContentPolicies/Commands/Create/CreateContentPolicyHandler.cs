using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Services.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ContentPolicies.Commands;

public class CreateContentPolicyHandler
    (
        IUnitOfWork uow,
        IContentPolicyRepository contentPolicyRepository,
        ContentPolicyRuleValidator ruleValidator,
        IContentPolicyCache contentPolicyCache)
    : IRequestHandler<CreateContentPolicyRequest, OperationResult<CreateContentPolicyResponse>>
{
    public async Task<OperationResult<CreateContentPolicyResponse>> Handle(CreateContentPolicyRequest request, CancellationToken cancellationToken)
    {
        var ruleDtos = request.Rules
            .Select(x => new ContentPolicyRuleDto(x.FieldPath, x.Operator, x.ValueType, x.Value, x.SortOrder, x.RuleGroup))
            .ToList();

        var errors = ruleValidator.ValidateRules(request.EntityType, ruleDtos);
        if (errors.Count > 0)
            return OperationResult<CreateContentPolicyResponse>.Fail();

        var policy = ContentPolicy.Create(
            request.RoleId,
            request.UserId,
            request.EntityType,
            request.Name,
            request.Priority,
            request.Effect,
            request.QueryAction,
            request.MergeMode);

        policy.Update(request.Name, request.Effect, request.IsActive, request.Priority, request.QueryAction);
        policy.AddRules(ruleDtos
            .Select(x => ContentPolicyRule.Create(x.FieldPath, x.Operator, x.ValueType, x.Value, x.SortOrder, x.RuleGroup))
            .ToArray());

        contentPolicyRepository.Add(policy);
        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return OperationResult<CreateContentPolicyResponse>.Fail();

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return new CreateContentPolicyResponse { Id = policy.Id };
    }
}
