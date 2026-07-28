using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Commands;

public record AdminChargeWalletRequest : IRequest<OperationResult<AdminChargeWalletResponse>>
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int WalletId { get; init; }

    public decimal Amount { get; init; }
    public string Description { get; init; } = default!;

    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int FinancialYearId { get; init; }
}
