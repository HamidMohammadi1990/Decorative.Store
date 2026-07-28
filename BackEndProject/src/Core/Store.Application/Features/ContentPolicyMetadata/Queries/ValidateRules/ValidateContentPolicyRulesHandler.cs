using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class ValidateContentPolicyRulesHandler
    (IContentPolicyMetadataRepository metadataRepository, IContentPolicyMapperService mapper)
    : IRequestHandler<ValidateContentPolicyRulesRequest, OperationResult<ValidateContentPolicyRulesResponse>>
{
    public Task<OperationResult<ValidateContentPolicyRulesResponse>> Handle(
        ValidateContentPolicyRulesRequest request,
        CancellationToken cancellationToken)
    {
        var requestDto = mapper.Map(request);
        var result = metadataRepository.ValidateRules(requestDto);
        return Task.FromResult<OperationResult<ValidateContentPolicyRulesResponse>>(mapper.Map(result));
    }
}
