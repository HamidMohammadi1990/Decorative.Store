using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Discounts.Queries;

public class GetAllDiscountHandler
    (IDiscountRepository discountRepository, IDiscountMapperService mapper)
    : IRequestHandler<GetAllDiscountRequest, PagedResult<GetAllDiscountResponse>>
{
    public async Task<PagedResult<GetAllDiscountResponse>> Handle(GetAllDiscountRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var discounts = await discountRepository.GetAllAsync(requestModel);
        return mapper.Map(discounts);
    }
}
