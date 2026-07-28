using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Discounts.Queries;

public class GetDiscountHandler
    (IDiscountRepository discountRepository, IDiscountMapperService mapper)
    : IRequestHandler<GetDiscountRequest, OperationResult<GetDiscountResponse?>>
{
    public async Task<OperationResult<GetDiscountResponse?>> Handle(GetDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = await discountRepository.GetAsNoTrackingAsync(request.Id);
        if (discount is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(discount);
    }
}
