namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record ValidateContentPolicyRulesResponse
{
    public bool IsValid { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}
