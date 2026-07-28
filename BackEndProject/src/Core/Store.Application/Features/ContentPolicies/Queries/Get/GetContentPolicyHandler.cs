using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicies.Queries;

public class GetContentPolicyHandler
    (IContentPolicyRepository contentPolicyRepository, IContentPolicyMapperService mapper)
    : IRequestHandler<GetContentPolicyRequest, OperationResult<GetContentPolicyResponse?>>
{
    public async Task<OperationResult<GetContentPolicyResponse?>> Handle(
        GetContentPolicyRequest request,
        CancellationToken cancellationToken)
    {
        var policy = await contentPolicyRepository.FindWithRulesAsync(request.Id, cancellationToken);
        if (policy is null)
            return default(GetContentPolicyResponse?);

        return mapper.Map(policy);
    }
}
