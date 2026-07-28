using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Queries;

public record GetAllWalletTransactionsRequest : ContentPolicyRequest<WalletTransaction>, IRequest<OperationResult<PagedResult<GetWalletTransactionResponse>>>
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int? WalletId { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int? UserId { get; init; }

    public WalletTransactionType? Type { get; init; }
    public WalletTransactionStatusType? Status { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
