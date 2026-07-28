using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Queries;

public class GetContentPolicyRecordAccessHandler
    (IContentPolicyRecordAccessRepository recordAccessRepository)
    : IRequestHandler<GetContentPolicyRecordAccessRequest, OperationResult<GetContentPolicyRecordAccessResponse?>>
{
    public async Task<OperationResult<GetContentPolicyRecordAccessResponse?>> Handle(
        GetContentPolicyRecordAccessRequest request,
        CancellationToken cancellationToken)
    {
        var recordAccess = await recordAccessRepository.FindWithPolicyAsync(request.Id, cancellationToken);
        if (recordAccess is null)
            return default(GetContentPolicyRecordAccessResponse?);

        return new GetContentPolicyRecordAccessResponse
        {
            Id = recordAccess.Id,
            PolicyId = recordAccess.PolicyId,
            EntityId = recordAccess.EntityId,
            EntityType = recordAccess.Policy.EntityType,
            PolicyName = recordAccess.Policy.Name,
            PolicyEffect = recordAccess.Policy.Effect
        };
    }
}
