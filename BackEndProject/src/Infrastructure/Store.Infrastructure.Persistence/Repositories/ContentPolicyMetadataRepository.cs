using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Services.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence.Repositories;

public class ContentPolicyMetadataRepository
    (
        IContentPolicyEntitySchemaExplorer schemaExplorer,
        ContentPolicyRuleValidator ruleValidator)
    : IContentPolicyMetadataRepository
{
    public IReadOnlyList<ContentPolicyEntityTypeOptionDto> GetEntityTypes()
        => schemaExplorer.GetEntityTypes();

    public IReadOnlyList<ContentPolicySchemaPropertyDto> GetEntitySchema(GetContentPolicyEntitySchemaRequestDto request)
        => schemaExplorer.GetProperties(request.EntityType, request.ParentPath);

    public IReadOnlyList<ContentPolicyOperator> GetAllowedOperators(GetContentPolicyPropertyOperatorsRequestDto request)
        => schemaExplorer.GetAllowedOperators(request.EntityType, request.FieldPath);

    public IReadOnlyList<ContentPolicyContextPathDto> GetContextPaths()
        => schemaExplorer.GetContextPaths();

    public ContentPolicyRuleValidationResultDto ValidateRules(ValidateContentPolicyRulesRequestDto request)
    {
        var errors = ruleValidator.ValidateRules(request.EntityType, request.Rules).ToList();

        foreach (var rule in request.Rules)
        {
            if (string.IsNullOrWhiteSpace(rule.FieldPath))
                continue;

            var allowed = schemaExplorer.GetAllowedOperators(request.EntityType, rule.FieldPath);
            if (allowed.Count > 0 && !allowed.Contains(rule.Operator))
                errors.Add($"Operator '{rule.Operator}' is not supported for path '{rule.FieldPath}'.");
        }

        return new ContentPolicyRuleValidationResultDto(errors.Count == 0, errors);
    }
}
