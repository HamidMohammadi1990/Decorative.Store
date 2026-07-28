using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class CompareContentPolicyMergeHandler
    (IContentPolicyPreviewService previewService, IContentPolicyMapperService mapper)
    : IRequestHandler<CompareContentPolicyMergeRequest, OperationResult<CompareContentPolicyMergeResponse>>
{
    public async Task<OperationResult<CompareContentPolicyMergeResponse>> Handle(
        CompareContentPolicyMergeRequest request,
        CancellationToken cancellationToken)
    {
        var requestDto = mapper.Map(request);
        var result = await previewService.CompareMergeAsync(requestDto, cancellationToken);
        if (result is null)
            return OperationResult<CompareContentPolicyMergeResponse>.Fail();

        return mapper.Map(result);
    }
}
