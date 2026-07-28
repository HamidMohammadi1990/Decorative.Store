using Store.Domain.Dtos.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Queries;

public class GetAllContentPolicyRecordAccessHandler
    (IContentPolicyRecordAccessRepository recordAccessRepository)
    : IRequestHandler<GetAllContentPolicyRecordAccessRequest, OperationResult<PagedResult<GetAllContentPolicyRecordAccessResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllContentPolicyRecordAccessResponse>>> Handle(
        GetAllContentPolicyRecordAccessRequest request,
        CancellationToken cancellationToken)
    {
        var dto = new GetAllContentPolicyRecordAccessRequestDto
        {
            PolicyId = request.PolicyId,
            EntityType = request.EntityType,
            EntityId = request.EntityId,
            Pagination = request.Pagination
        };

        var records = await recordAccessRepository.GetAllAsync(dto, cancellationToken);
        var items = records.Items
            .Select(x => new GetAllContentPolicyRecordAccessResponse
            {
                Id = x.Id,
                PolicyId = x.PolicyId,
                EntityId = x.EntityId,
                PolicyName = x.Policy.Name,
                EntityType = x.Policy.EntityType,
                PolicyEffect = x.Policy.Effect
            })
            .ToList();

        return PagedResult<GetAllContentPolicyRecordAccessResponse>.Create(items, records);
    }
}
