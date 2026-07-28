namespace Store.Domain.Dtos.Discounts;

public record GetAllDiscountResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = null!;
    public int? UserId { get; init; }
    public int? ProductId { get; init; }
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
    public bool IsActive { get; init; } = true;
}