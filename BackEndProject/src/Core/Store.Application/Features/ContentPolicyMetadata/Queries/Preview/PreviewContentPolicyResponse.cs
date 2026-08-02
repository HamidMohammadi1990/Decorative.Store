using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record PreviewContentPolicyResponse
{
    public int UserId { get; init; }
    public string EntityType { get; init; }
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.GetAll;
    public ContentPolicyAccessMode AccessMode { get; init; }
    public ContentPolicyMergeMode EffectiveMergeMode { get; init; }
    public bool BypassContentPolicy { get; init; }
    public bool RequireContentPolicy { get; init; }
    public IReadOnlyList<UserRolePolicyDto> Roles { get; init; } = [];
    public IReadOnlyList<ContentPolicyPreviewPolicyDto> AppliedPolicies { get; init; } = [];
    public IReadOnlyList<ContentPolicyPreviewPolicyDto> ExcludedRolePolicies { get; init; } = [];
    public int TotalEntityCount { get; init; }
    public int AccessibleEntityCount { get; init; }
    public IReadOnlyList<int> SampleAccessibleIds { get; init; } = [];
}
