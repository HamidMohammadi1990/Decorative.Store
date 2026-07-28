using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyRules.Commands;

public class DeleteContentPolicyRuleHandler
    (IUnitOfWork uow, IContentPolicyRuleRepository contentPolicyRuleRepository, IContentPolicyCache contentPolicyCache)
    : IRequestHandler<DeleteContentPolicyRuleRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteContentPolicyRuleRequest request, CancellationToken cancellationToken)
    {
        var rule = await contentPolicyRuleRepository.FindAsync(request.Id, cancellationToken);
        if (rule is null)
            return OperationResult.Fail();

        contentPolicyRuleRepository.Remove(rule);
        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return OperationResult.Fail();

        await contentPolicyCache.InvalidateAllAsync(cancellationToken);
        return OperationResult.Success();
    }
}
