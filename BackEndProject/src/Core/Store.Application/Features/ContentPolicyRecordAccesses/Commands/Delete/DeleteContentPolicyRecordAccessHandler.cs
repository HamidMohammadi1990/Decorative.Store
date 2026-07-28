using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Commands;

public class DeleteContentPolicyRecordAccessHandler
    (
        IUnitOfWork uow,
        IContentPolicyRecordAccessRepository recordAccessRepository,
        IContentPolicyCache contentPolicyCache)
    : IRequestHandler<DeleteContentPolicyRecordAccessRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteContentPolicyRecordAccessRequest request, CancellationToken cancellationToken)
    {
        var recordAccess = await recordAccessRepository.FindAsync(request.Id, cancellationToken);
        if (recordAccess is null)
            return ErrorModel.Create("InvalidId");

        recordAccessRepository.Remove(recordAccess);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return OperationResult.Success();
    }
}
