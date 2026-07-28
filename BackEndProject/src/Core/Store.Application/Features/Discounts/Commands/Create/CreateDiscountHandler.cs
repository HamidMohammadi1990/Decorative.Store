using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Discounts.Commands;

public class CreateDiscountHandler
    (IUnitOfWork uow, IDiscountRepository discountRepository)
    : IRequestHandler<CreateDiscountRequest, OperationResult<CreateDiscountResponse>>
{
    public async Task<OperationResult<CreateDiscountResponse>> Handle(CreateDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = Discount.Create(
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

        discountRepository.Add(discount);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateDiscountResponse>();

        return new CreateDiscountResponse { Id = discount.Id };
    }
}