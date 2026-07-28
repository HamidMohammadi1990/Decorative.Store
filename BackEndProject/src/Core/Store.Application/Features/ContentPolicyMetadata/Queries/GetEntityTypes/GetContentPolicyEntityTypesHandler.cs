using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class GetContentPolicyEntityTypesHandler(IContentPolicyMetadataRepository metadataRepository)
    : IRequestHandler<GetContentPolicyEntityTypesRequest, OperationResult<GetContentPolicyEntityTypesResponse>>
{
    public Task<OperationResult<GetContentPolicyEntityTypesResponse>> Handle(
        GetContentPolicyEntityTypesRequest request,
        CancellationToken cancellationToken)
        => Task.FromResult<OperationResult<GetContentPolicyEntityTypesResponse>>(
            new GetContentPolicyEntityTypesResponse { EntityTypes = metadataRepository.GetEntityTypes() });
}
