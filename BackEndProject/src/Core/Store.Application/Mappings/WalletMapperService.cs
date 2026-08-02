using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Wallets.Queries;
using Edition.Application.Features.Wallets.Commands;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class WalletMapperService : IWalletMapperService
{
    public GetWalletResponse Map(Wallet model)
    {
        return new GetWalletResponse
        {
            Id = model.Id,
            Title = model.Title,
            UserId = model.UserId,
            Balance = model.Balance,
            IsDefault = model.IsDefault,
            Status = model.Status,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }

    public GetMyWalletResponse MapToMyWallet(Wallet model)
    {
        return new GetMyWalletResponse
        {
            Id = model.Id,
            Title = model.Title,
            Balance = model.Balance,
            IsDefault = model.IsDefault,
            Status = model.Status,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }

    public List<GetMyWalletResponse> MapToMyWallets(IEnumerable<Wallet> model)
        => model.Select(MapToMyWallet).ToList();

    public PagedResult<GetAllWalletResponse> Map(PagedResult<Wallet> model)
    {
        var items = model.Items.Select(x => new GetAllWalletResponse
        {
            Id = x.Id,
            Title = x.Title,
            UserId = x.UserId,
            Balance = x.Balance,
            IsDefault = x.IsDefault,
            Status = x.Status,
            CreatedOnUtc = x.CreatedOnUtc
        }).ToList();

        return PagedResult<GetAllWalletResponse>.Create(items, model);
    }

    public PagedResult<SearchWalletResponse> MapToSearch(PagedResult<Wallet> model)
    {
        var items = model.Items.Select(x => new SearchWalletResponse
        {
            Id = x.Id,
            Title = x.Title,
            Balance = x.Balance,
            IsDefault = x.IsDefault
        }).ToList();

        return PagedResult<SearchWalletResponse>.Create(items, model);
    }

    public PagedResult<GetWalletTransactionResponse> MapTransactions(PagedResult<WalletTransaction> model)
    {
        var items = model.Items.Select(MapTransaction).ToList();
        return PagedResult<GetWalletTransactionResponse>.Create(items, model);
    }

    public VerifyWalletChargeResponse MapVerifyCharge(WalletPendingBankCharge charge, bool isPaymentSuccessful)
    {
        return new VerifyWalletChargeResponse
        {
            IsPaymentSuccessful = isPaymentSuccessful,
            WalletId = charge.Wallet.Id,
            Amount = charge.WalletTransaction.Amount,
            NewBalance = charge.Wallet.Balance
        };
    }

    public GetAllWalletRequestDto Map(GetAllWalletRequest model)
    {
        return new GetAllWalletRequestDto
        {
            Title = model.Title,
            UserId = model.UserId,
            Status = model.Status,
            IsDefault = model.IsDefault,
            Pagination = model.Pagination
        }.WithContentPolicy<Wallet, GetAllWalletRequestDto>(model);
    }

    public SearchWalletRequestDto Map(SearchWalletRequest model)
    {
        return new SearchWalletRequestDto
        {
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<Wallet, SearchWalletRequestDto>(model);
    }

    public GetWalletTransactionsRequestDto Map(GetMyWalletTransactionsRequest model)
    {
        return new GetWalletTransactionsRequestDto
        {
            WalletId = model.WalletId,
            Type = model.Type,
            Status = model.Status,
            Pagination = model.Pagination
        }.WithContentPolicy<WalletTransaction, GetWalletTransactionsRequestDto>(model);
    }

    public GetWalletTransactionsRequestDto Map(GetAllWalletTransactionsRequest model, int? userId = null)
    {
        return new GetWalletTransactionsRequestDto
        {
            WalletId = model.WalletId,
            UserId = userId ?? model.UserId,
            Type = model.Type,
            Status = model.Status,
            Pagination = model.Pagination
        }.WithContentPolicy<WalletTransaction, GetWalletTransactionsRequestDto>(model);
    }

    private static GetWalletTransactionResponse MapTransaction(WalletTransaction model)
    {
        return new GetWalletTransactionResponse
        {
            Id = model.Id,
            WalletId = model.WalletId,
            Amount = model.Amount,
            Description = model.Description,
            Type = model.Type,
            Status = model.Status,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }
}
