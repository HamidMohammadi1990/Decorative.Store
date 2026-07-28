using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Discounts.Commands;

public class UpdateDiscountHandler
    (IUnitOfWork uow, IDiscountRepository discountRepository)
    : IRequestHandler<UpdateDiscountRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = await discountRepository.FindAsync(request.Id);
        if (discount is null)
            return ErrorModel.Create("InvalidId");

        discount.Update(
            request.Code,
            request.UserId,
            request.ProductId,
            request.SubCategoryId,
            request.Percentage,
            request.Amount,
            request.ExpiryDateOnUtc,
            request.MaxDiscountAmount,
            request.FromCirculationOrMeterOrCount,
            request.ToCirculationOrMeterOrCount,
            request.UsageLimit,
            request.RemainingUses,
            request.IsCooperation,
            request.MinimumAmount,
            request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}