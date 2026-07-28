using Edition.Application.Features.Wallets.Commands;
using Edition.Application.Features.Wallets.Queries;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IWalletMapperService : IMapper
{
    GetWalletResponse Map(Wallet model);
    GetMyWalletResponse MapToMyWallet(Wallet model);
    List<GetMyWalletResponse> MapToMyWallets(IEnumerable<Wallet> model);
    PagedResult<GetAllWalletResponse> Map(PagedResult<Wallet> model);
    PagedResult<SearchWalletResponse> MapToSearch(PagedResult<Wallet> model);
    PagedResult<GetWalletTransactionResponse> MapTransactions(PagedResult<WalletTransaction> model);
    VerifyWalletChargeResponse MapVerifyCharge(WalletPendingBankCharge charge, bool isPaymentSuccessful);
    GetAllWalletRequestDto Map(GetAllWalletRequest model);
    SearchWalletRequestDto Map(SearchWalletRequest model);
    GetWalletTransactionsRequestDto Map(GetMyWalletTransactionsRequest model);
    GetWalletTransactionsRequestDto Map(GetAllWalletTransactionsRequest model, int? userId = null);
}
