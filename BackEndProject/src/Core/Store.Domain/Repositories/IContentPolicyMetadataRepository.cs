using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Store.Domain.Repositories;

public interface IContentPolicyMetadataRepository
{
    IReadOnlyList<ContentPolicyEntityTypeOptionDto> GetEntityTypes();
    IReadOnlyList<ContentPolicySchemaPropertyDto> GetEntitySchema(GetContentPolicyEntitySchemaRequestDto request);
    IReadOnlyList<ContentPolicyOperator> GetAllowedOperators(GetContentPolicyPropertyOperatorsRequestDto request);
    IReadOnlyList<ContentPolicyContextPathDto> GetContextPaths();
    ContentPolicyRuleValidationResultDto ValidateRules(ValidateContentPolicyRulesRequestDto request);
}
