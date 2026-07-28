using Store.Domain.Enums;

namespace Store.Domain.Dtos.ContentPolicies;

public record GetContentPolicyEntitySchemaRequestDto
{
    public string EntityType { get; init; } = default!;
    public string? ParentPath { get; init; }
}

public record GetContentPolicyPropertyOperatorsRequestDto
{
    public string EntityType { get; init; } = default!;
    public string FieldPath { get; init; } = default!;
}

public record GetContentPolicyPolicySetsRequestDto
{
    public string EntityType { get; init; } = default!;
    public ContentPolicyQueryAction QueryAction { get; init; }
    public int UserId { get; init; }
    public IReadOnlyList<int> RoleIds { get; init; } = [];
}

public record PreviewContentPolicyRequestDto
{
    public int UserId { get; init; }
    public string EntityType { get; init; } = default!;
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.GetAll;
    public int SampleSize { get; init; } = 10;
}

public record CompareContentPolicyDraftPolicyDto
{
    public string Name { get; init; } = "Draft";
    public ContentPolicyEffect Effect { get; init; } = ContentPolicyEffect.Allow;
    public ContentPolicyMergeMode MergeMode { get; init; } = ContentPolicyMergeMode.Additive;
    public int Priority { get; init; }
    public IReadOnlyList<ContentPolicyRuleDto> Rules { get; init; } = [];
}

public record CompareContentPolicyMergeRequestDto
{
    public int UserId { get; init; }
    public string EntityType { get; init; } = default!;
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.GetAll;
    public int SampleSize { get; init; } = 10;
    public CompareContentPolicyDraftPolicyDto? DraftUserPolicy { get; init; }
}

public record ValidateContentPolicyRulesRequestDto
{
    public string EntityType { get; init; } = default!;
    public IReadOnlyList<ContentPolicyRuleDto> Rules { get; init; } = [];
}
