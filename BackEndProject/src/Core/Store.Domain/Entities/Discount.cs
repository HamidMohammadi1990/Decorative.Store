using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Discount : BaseEntity
{
    public string Code { get; private set; } = null!;
    public int? UserId { get; private set; }
    public int? ProductId { get; private set; }
    public int? SubCategoryId { get; private set; }
    public int Percentage { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime? ExpiryDateOnUtc { get; private set; }
    public decimal MaxDiscountAmount { get; private set; }
    public int? FromCirculationOrMeterOrCount { get; private set; }
    public int? ToCirculationOrMeterOrCount { get; private set; }
    public int UsageLimit { get; private set; }
    public int RemainingUses { get; private set; }
    public bool IsCooperation { get; private set; }
    public decimal? MinimumAmount { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Product Product { get; private set; } = default!;
    public User User { get; private set; } = default!;
    public SubCategory SubCategory { get; private set; } = default!;


    public static Discount Create(string code, int? userId, int? productId, int? subCategoryId, int percentage, decimal amount, DateTime? expiryDateOnUtc, decimal maxDiscountAmount, int? fromCirculationOrMeterOrCount, int? toCirculationOrMeterOrCount, int usageLimit, int remainingUses, bool isCooperation, decimal? minimumAmount, bool isActive)
        => new()
        {
            Code = code,
            UserId = userId,
            ProductId = productId,
            SubCategoryId = subCategoryId,
            Percentage = percentage,
            Amount = amount,
            ExpiryDateOnUtc = expiryDateOnUtc,
            MaxDiscountAmount = maxDiscountAmount,
            FromCirculationOrMeterOrCount = fromCirculationOrMeterOrCount,
            ToCirculationOrMeterOrCount = toCirculationOrMeterOrCount,
            UsageLimit = usageLimit,
            RemainingUses = remainingUses,
            IsCooperation = isCooperation,
            MinimumAmount = minimumAmount,
            IsActive = isActive
        };

    public void Update(string code, int? userId, int? productId, int? subCategoryId, int percentage, decimal amount, DateTime? expiryDateOnUtc, decimal maxDiscountAmount, int? fromCirculationOrMeterOrCount, int? toCirculationOrMeterOrCount, int usageLimit, int remainingUses, bool isCooperation, decimal? minimumAmount, bool isActive)    
    {
        Code = code;
        UserId = userId;
        ProductId = productId;
        SubCategoryId = subCategoryId;
        Percentage = percentage;
        Amount = amount;
        ExpiryDateOnUtc = expiryDateOnUtc;
        MaxDiscountAmount = maxDiscountAmount;
        FromCirculationOrMeterOrCount = fromCirculationOrMeterOrCount;
        ToCirculationOrMeterOrCount = toCirculationOrMeterOrCount;
        UsageLimit = usageLimit;
        RemainingUses = remainingUses;
        IsCooperation = isCooperation;
        MinimumAmount = minimumAmount;
        IsActive = isActive;
    }

    public void MinuseRemainingUses()
    {
        RemainingUses--;
    }

    public void RestoreUsage()
    {
        if (RemainingUses < UsageLimit)
            RemainingUses++;
    }

    public void ConsumeUsage()
    {
        if (RemainingUses <= 0)
            throw new InvalidOperationException("Discount usage limit has been reached.");

        MinuseRemainingUses();
    }
}