using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicies.Queries;

public class GetAllContentPolicyHandler
    (IContentPolicyRepository contentPolicyRepository, IContentPolicyMapperService mapper)
    : IRequestHandler<GetAllContentPolicyRequest, OperationResult<PagedResult<GetAllContentPolicyResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllContentPolicyResponse>>> Handle(
        GetAllContentPolicyRequest request,
        CancellationToken cancellationToken)
    {
        var requestDto = mapper.Map(request);
        var policies = await contentPolicyRepository.GetAllAsync(requestDto, cancellationToken);
        return mapper.Map(policies);
    }
}
