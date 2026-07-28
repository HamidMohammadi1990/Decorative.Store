using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record CompareContentPolicyMergeRequest : IRequest<OperationResult<CompareContentPolicyMergeResponse>>
{
    public int UserId { get; init; }
    public string EntityType { get; init; }
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.GetAll;
    public int SampleSize { get; init; } = 10;
    public CompareContentPolicyDraftPolicy? DraftUserPolicy { get; init; }
}

public record CompareContentPolicyDraftPolicy
{
    public string Name { get; init; } = "Draft";
    public ContentPolicyEffect Effect { get; init; } = ContentPolicyEffect.Allow;
    public ContentPolicyMergeMode MergeMode { get; init; } = ContentPolicyMergeMode.Additive;
    public int Priority { get; init; }
    public List<ValidateContentPolicyRuleInput> Rules { get; init; } = [];
}
