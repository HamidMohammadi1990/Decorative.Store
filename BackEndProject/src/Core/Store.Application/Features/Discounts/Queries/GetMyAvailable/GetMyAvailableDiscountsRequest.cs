using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.Discounts.Queries;

public record GetMyAvailableDiscountsRequest
    : IRequest<OperationResult<GetMyAvailableDiscountsResponse>>;

public record GetMyAvailableDiscountsResponse
{
    public List<GetMyAvailableDiscountItemResponse> Items { get; init; } = [];
}

public record GetMyAvailableDiscountItemResponse
{
    [JsonConverter(typeof(DiscountEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;
    public int Percentage { get; init; }
    public decimal Amount { get; init; }
    public DateTime? ExpiryDateOnUtc { get; init; }
    public decimal MaxDiscountAmount { get; init; }
    public int UsageLimit { get; init; }
    public int RemainingUses { get; init; }
    public decimal? MinimumAmount { get; init; }
    public bool IsActive { get; init; }
    public bool IsPersonal { get; init; }
}
