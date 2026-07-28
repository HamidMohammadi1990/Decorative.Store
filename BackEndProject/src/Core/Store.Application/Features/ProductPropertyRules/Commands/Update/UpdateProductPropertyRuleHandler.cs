using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public class UpdateProductPropertyRuleHandler
    (IUnitOfWork uow, IProductPropertyRuleRepository productPropertyRuleRepository)
    : IRequestHandler<UpdateProductPropertyRuleRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var productPropertyRule = await productPropertyRuleRepository.FindAsync(request.Id);
        if (productPropertyRule is null)
            return ErrorModel.Create("InvalidId");

        productPropertyRule.Update(
            request.IsMandatory,
            request.Description,
            request.ProductPropertyId,
            request.IsActive,
            request.MinLength,
            request.MaxLength,
            request.MinQuantity,
            request.MaxQuantity,
            request.MinWidth,
            request.MaxWidth,
            request.MinHeight,
            request.MaxHeight);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
