using Store.Domain.Dtos.ContentPolicies;

namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyPreviewService
{
    Task<ContentPolicyPreviewResultDto?> PreviewAsync(
        PreviewContentPolicyRequestDto request,
        CancellationToken cancellationToken = default);

    Task<ContentPolicyMergeCompareResultDto?> CompareMergeAsync(
        CompareContentPolicyMergeRequestDto request,
        CancellationToken cancellationToken = default);
}
