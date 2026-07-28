using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyRules.Queries;

public class GetContentPolicyRuleHandler
    (IContentPolicyRuleRepository contentPolicyRuleRepository)
    : IRequestHandler<GetContentPolicyRuleRequest, OperationResult<GetContentPolicyRuleResponse?>>
{
    public async Task<OperationResult<GetContentPolicyRuleResponse?>> Handle(GetContentPolicyRuleRequest request, CancellationToken cancellationToken)
    {
        var rule = await contentPolicyRuleRepository.FindWithPolicyAsync(request.Id, cancellationToken);
        if (rule is null)
            return default(GetContentPolicyRuleResponse?);

        return new GetContentPolicyRuleResponse
        {
            Id = rule.Id,
            Value = rule.Value,
            PolicyId = rule.PolicyId,
            Operator = rule.Operator,
            FieldPath = rule.FieldPath,
            ValueType = rule.ValueType,
            SortOrder = rule.SortOrder,
            RuleGroup = rule.RuleGroup,
            PolicyName = rule.Policy.Name,
            EntityType = rule.Policy.EntityType
        };
    }
}
