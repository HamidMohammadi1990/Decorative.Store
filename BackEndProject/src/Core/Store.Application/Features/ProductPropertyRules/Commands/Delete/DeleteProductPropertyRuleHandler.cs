using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public class DeleteProductPropertyRuleHandler
    (IUnitOfWork uow, IProductPropertyRuleRepository productPropertyRuleRepository)
    : IRequestHandler<DeleteProductPropertyRuleRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var productPropertyRule = await productPropertyRuleRepository.FindAsync(request.Id);
        if (productPropertyRule is null)
            return ErrorModel.Create("InvalidId");

        productPropertyRuleRepository.Remove(productPropertyRule);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
