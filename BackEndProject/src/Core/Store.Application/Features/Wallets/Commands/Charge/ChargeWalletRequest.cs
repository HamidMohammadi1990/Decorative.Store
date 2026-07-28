using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Commands;

public record ChargeWalletRequest : IRequest<OperationResult<ChargeWalletResponse>>
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int WalletId { get; init; }

    public decimal Amount { get; init; }

    [JsonConverter(typeof(BankAccountEncryptor))]
    public int BankId { get; init; }

    public string? Description { get; init; }
}
