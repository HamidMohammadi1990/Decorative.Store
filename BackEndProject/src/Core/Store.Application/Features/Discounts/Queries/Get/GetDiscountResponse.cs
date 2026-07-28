using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Discounts.Queries;

public record GetDiscountResponse
{
    [JsonConverter(typeof(DiscountEncryptor))]
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }
    [JsonConverter(typeof(SubCategoryNullableEncryptor))]
    public int? SubCategoryId { get; init; }
    public int Percentage { get; init; }
    public decimal Amount { get; init; }
    public DateTime? ExpiryDateOnUtc { get; init; }
    public decimal MaxDiscountAmount { get; init; }
    public int? FromCirculationOrMeterOrCount { get; init; }
    public int? ToCirculationOrMeterOrCount { get; init; }
    public int UsageLimit { get; init; }
    public int RemainingUses { get; init; }
    public bool IsCooperation { get; init; }
    public decimal? MinimumAmount { get; init; }
    public bool IsActive { get; init; }
}
