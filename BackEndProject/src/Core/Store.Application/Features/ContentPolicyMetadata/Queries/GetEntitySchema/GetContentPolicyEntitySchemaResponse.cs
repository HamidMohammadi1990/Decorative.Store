using Edition.Domain.Enums;
using Store.Domain.Dtos.ContentPolicies;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record GetContentPolicyEntitySchemaResponse
{
    public string EntityType { get; init; }
    public string? ParentPath { get; init; }
    public IReadOnlyList<ContentPolicySchemaPropertyDto> Properties { get; init; } = [];
}
