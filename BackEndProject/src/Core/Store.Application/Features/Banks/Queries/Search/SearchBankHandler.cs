using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Banks.Queries;

public class SearchBankHandler
    (IBankRepository bankRepository, IBankMapperService mapper)
    : IRequestHandler<SearchBankRequest, OperationResult<PagedResult<SearchBankResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchBankResponse>>> Handle(SearchBankRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var banks = await bankRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(banks);
    }
}
