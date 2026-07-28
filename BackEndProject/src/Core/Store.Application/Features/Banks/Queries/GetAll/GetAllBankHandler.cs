using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Banks.Queries;

public class GetAllBankHandler
    (IBankRepository bankRepository, IBankMapperService mapper)
    : IRequestHandler<GetAllBankRequest, OperationResult<PagedResult<GetAllBankResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBankResponse>>> Handle(GetAllBankRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var banks = await bankRepository.GetAllAsync(requestModel);
        return mapper.Map(banks);
    }
}
