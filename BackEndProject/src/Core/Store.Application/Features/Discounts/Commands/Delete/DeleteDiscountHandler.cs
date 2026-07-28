using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Discounts.Commands;

public class DeleteDiscountHandler
    (IUnitOfWork uow, IDiscountRepository discountRepository)
    : IRequestHandler<DeleteDiscountRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = await discountRepository.FindAsync(request.Id);
        if (discount is null)
            return ErrorModel.Create("InvalidId");

        discountRepository.Remove(discount);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}