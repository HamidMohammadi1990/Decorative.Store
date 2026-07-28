using Edition.Application.Contracts.Mapping;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Wallets.Queries;

public class SearchWalletHandler
    (IWalletRepository walletRepository, IWalletMapperService mapper)
    : IRequestHandler<SearchWalletRequest, OperationResult<PagedResult<SearchWalletResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchWalletResponse>>> Handle(SearchWalletRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var wallets = await walletRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(wallets);
    }
}