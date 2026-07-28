using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Wallets.Queries;

public class GetAllWalletHandler
    (IWalletRepository walletRepository, IWalletMapperService mapper)
    : IRequestHandler<GetAllWalletRequest, OperationResult<PagedResult<GetAllWalletResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllWalletResponse>>> Handle(GetAllWalletRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var wallets = await walletRepository.GetAllAsync(requestModel);
        return mapper.Map(wallets);
    }
}