using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record GetContentPolicyEntityTypesRequest : IRequest<OperationResult<GetContentPolicyEntityTypesResponse>>;
