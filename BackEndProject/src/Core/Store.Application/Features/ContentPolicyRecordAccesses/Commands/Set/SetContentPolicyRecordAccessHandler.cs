using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Services.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Commands;

public class SetContentPolicyRecordAccessHandler
    (
        IUnitOfWork uow,
        IContentPolicyRepository contentPolicyRepository,
        IContentPolicyRecordAccessRepository recordAccessRepository,
        ContentPolicyRecordAccessValidator recordAccessValidator,
        IContentPolicyCache contentPolicyCache)
    : IRequestHandler<SetContentPolicyRecordAccessRequest, OperationResult>
{
    public async Task<OperationResult> Handle(SetContentPolicyRecordAccessRequest request, CancellationToken cancellationToken)
    {
        var policy = await contentPolicyRepository.FindAsync(request.PolicyId, cancellationToken);
        if (policy is null)
            return ErrorModel.Create("InvalidId");

        var entityIds = request.EntityIds
            .Where(x => x > 0)
            .Distinct()
            .ToList();

        foreach (var entityId in entityIds)
        {
            var validationError = await recordAccessValidator.ValidateEntityExistsAsync(
                policy.EntityType,
                entityId,
                cancellationToken);
            if (validationError is not null)
                return ErrorModel.CreateLiteral("InvalidReference", validationError);
        }

        var existing = await recordAccessRepository.GetByPolicyIdAsync(request.PolicyId, cancellationToken);
        recordAccessRepository.RemoveRange(existing);

        foreach (var entityId in entityIds)
            recordAccessRepository.Add(ContentPolicyRecordAccess.CreateForPolicy(request.PolicyId, entityId));

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return OperationResult.Success();
    }
}
