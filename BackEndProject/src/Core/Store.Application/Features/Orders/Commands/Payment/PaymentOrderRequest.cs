using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public record PaymentOrderRequest : IRequest<OperationResult<PaymentOrderResponse>>
{
    [JsonConverter(typeof(BankAccountEncryptor))]
    public int? BankId { get; init; }

    [JsonConverter(typeof(WalletEncryptor))]
    public int? WalletId { get; init; }

    public string? DiscountCode { get; init; }
    public PaymentOptionType PaymentOption { get; init; }
}