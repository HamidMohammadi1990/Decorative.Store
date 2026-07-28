using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyEntitySchemaExplorer
{
    IReadOnlyList<ContentPolicyEntityTypeOptionDto> GetEntityTypes();
    IReadOnlyList<ContentPolicySchemaPropertyDto> GetProperties(string entityType, string? parentPath = null);
    IReadOnlyList<ContentPolicyOperator> GetAllowedOperators(string entityType, string fieldPath);
    IReadOnlyList<ContentPolicyContextPathDto> GetContextPaths();
}
