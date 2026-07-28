using Store.Domain.Dtos.ContentPolicies;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record GetContentPolicyEntityTypesResponse
{
    public IReadOnlyList<ContentPolicyEntityTypeOptionDto> EntityTypes { get; init; } = [];
}
