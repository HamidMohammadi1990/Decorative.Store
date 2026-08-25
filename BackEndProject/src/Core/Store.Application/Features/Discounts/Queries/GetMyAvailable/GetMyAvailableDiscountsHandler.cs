using Edition.Application.Contracts;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Discounts.Queries;

public class GetMyAvailableDiscountsHandler
    (ICurrentUserContext currentUser, IDiscountRepository discountRepository)
    : IRequestHandler<GetMyAvailableDiscountsRequest, OperationResult<GetMyAvailableDiscountsResponse>>
{
    public async Task<OperationResult<GetMyAvailableDiscountsResponse>> Handle(
        GetMyAvailableDiscountsRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var discounts = await discountRepository.GetAvailableForUserAsync(userId, cancellationToken);

        return new GetMyAvailableDiscountsResponse
        {
            Items = discounts.Select(x => new GetMyAvailableDiscountItemResponse
            {
                Id = x.Id,
                Code = x.Code,
                Percentage = x.Percentage,
                Amount = x.Amount,
                ExpiryDateOnUtc = x.ExpiryDateOnUtc,
                MaxDiscountAmount = x.MaxDiscountAmount,
                UsageLimit = x.UsageLimit,
                RemainingUses = x.RemainingUses,
                MinimumAmount = x.MinimumAmount,
                IsActive = x.IsActive,
                IsPersonal = x.UserId == userId,
            }).ToList(),
        };
    }
}
