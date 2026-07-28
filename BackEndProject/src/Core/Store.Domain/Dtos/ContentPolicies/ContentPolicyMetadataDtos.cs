using Store.Domain.Enums;

namespace Store.Domain.Dtos.ContentPolicies;

public record ContentPolicySchemaPropertyDto(
    string Name,
    string Path,
    ContentPolicyPropertyKind Kind,
    string ClrTypeName,
    bool IsNullable,
    bool IsExpandable,
    IReadOnlyList<ContentPolicyOperator> AllowedOperators);

public record ContentPolicyContextPathDto(
    string Path,
    string ClrTypeName,
    bool IsCollection);

public record ContentPolicyEnumOptionDto(
    int Value,
    string Name);

public record ContentPolicyEntityTypeOptionDto(string Name);

public record ContentPolicyRuleValidationResultDto(
    bool IsValid,
    IReadOnlyList<string> Errors);
