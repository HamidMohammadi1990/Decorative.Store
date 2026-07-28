using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicies.Commands;

public class DeleteContentPolicyHandler
    (IUnitOfWork uow, IContentPolicyRepository contentPolicyRepository, IContentPolicyCache contentPolicyCache)
    : IRequestHandler<DeleteContentPolicyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteContentPolicyRequest request, CancellationToken cancellationToken)
    {
        var policy = await contentPolicyRepository.FindAsync(request.Id, cancellationToken);
        if (policy is null)
            return OperationResult.Fail();

        contentPolicyRepository.Remove(policy);
        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return OperationResult.Fail();

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return OperationResult.Success();
    }
}
