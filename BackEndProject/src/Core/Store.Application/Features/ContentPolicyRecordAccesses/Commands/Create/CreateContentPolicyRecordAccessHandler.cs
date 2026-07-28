using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Services.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Commands;

public class CreateContentPolicyRecordAccessHandler
    (
        IUnitOfWork uow,
        IContentPolicyRepository contentPolicyRepository,
        IContentPolicyRecordAccessRepository recordAccessRepository,
        ContentPolicyRecordAccessValidator recordAccessValidator,
        IContentPolicyCache contentPolicyCache)
    : IRequestHandler<CreateContentPolicyRecordAccessRequest, OperationResult<CreateContentPolicyRecordAccessResponse>>
{
    public async Task<OperationResult<CreateContentPolicyRecordAccessResponse>> Handle(
        CreateContentPolicyRecordAccessRequest request,
        CancellationToken cancellationToken)
    {
        var policy = await contentPolicyRepository.FindAsync(request.PolicyId, cancellationToken);
        if (policy is null)
            return ErrorModel.Create("InvalidId");

        if (await recordAccessRepository.ExistsAsync(request.PolicyId, request.EntityId, cancellationToken))
            return ErrorModel.Create("DuplicateRecord");

        var validationError = await recordAccessValidator.ValidateEntityExistsAsync(
            policy.EntityType,
            request.EntityId,
            cancellationToken);
        if (validationError is not null)
            return ErrorModel.CreateLiteral("InvalidReference", validationError);

        var recordAccess = ContentPolicyRecordAccess.CreateForPolicy(request.PolicyId, request.EntityId);
        recordAccessRepository.Add(recordAccess);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.ToGenericFailure<CreateContentPolicyRecordAccessResponse>();

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return new CreateContentPolicyRecordAccessResponse { Id = recordAccess.Id };
    }
}
