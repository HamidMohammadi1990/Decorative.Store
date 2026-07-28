using Store.Domain.Enums;
using Store.Domain.Common;
using Store.Domain.Dtos.Orders;

namespace Store.Domain.Entities;

public class Order : BaseEntity
{
    public long TrackingCode { get; private set; }
    public string Title { get; private set; } = null!;
    public int UserId { get; private set; }
    public OrderStatusType Status { get; private set; } = OrderStatusType.Pending;
    public bool IsFinaly { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; private set; }
    public decimal FinalPrice { get; private set; }
    public decimal VatPrice { get; private set; }
    public decimal TotalCommissionPrice { get; private set; }
    public int? AppliedDiscountId { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public bool IsDiscountUsageConsumed { get; private set; }


    public User User { get; private set; } = default!;
    public Discount? AppliedDiscount { get; private set; }
    public ICollection<OrderVat> OrderVats { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; private set; } = [];
    public ICollection<OrderNote> OrderNotes { get; private set; } = default!;
    public ICollection<OrderCommission> OrderCommissions { get; set; } = default!;
    public ICollection<FinancialDocument> FinancialDocuments { get; set; } = default!;


    public static Order Create(string title, int userId)
        => new()
        {
            Title = title,
            UserId = userId
        };

    public void AssignTrackingCode(long trackingCode)
    {
        if (trackingCode <= 0)
            throw new ArgumentOutOfRangeException(nameof(trackingCode));

        if (TrackingCode != 0)
            throw new InvalidOperationException("Tracking code is already assigned.");

        TrackingCode = trackingCode;
    }

    public void SetTotalPrice(decimal totalPrice)
    {
        TotalPrice = totalPrice;
    }

    public void SetFinalPrice(decimal finalPrice)
    {
        FinalPrice = finalPrice;
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        OrderItems.Add(orderItem);
    }

    public bool RemoveOrderItem(OrderItem orderItem)
    {
        return OrderItems.Remove(orderItem);
    }

    public bool IsEmpty => OrderItems.Count == 0;

    public bool CanBeDeletedAsEmptyCart()
        => Status == OrderStatusType.Pending && IsEmpty;

    public void RecalculateTotalPrice()
    {
        TotalPrice = OrderItems.Sum(item => item.GetSumPrices() * item.Quantity);
    }

    public void RecalculateVatPrice(decimal vatRate)
    {
        var netAmount = Math.Max(0m, TotalPrice - DiscountAmount);
        VatPrice = Math.Round(netAmount * vatRate, 2, MidpointRounding.AwayFromZero);
        FinalPrice = netAmount + VatPrice;
    }

    public void CompletePayment()
    {
        if (Status != OrderStatusType.Pending)
            throw new InvalidOperationException("Only pending orders can be paid.");

        Status = OrderStatusType.InProgress;
        IsFinaly = true;
    }

    public void ClearDiscountApplication()
    {
        AppliedDiscountId = null;
        DiscountAmount = 0;
        FinalPrice = TotalPrice;
    }

    public void SetDiscountPreview(int discountId, decimal discountAmount)
    {
        AppliedDiscountId = discountId;
        DiscountAmount = discountAmount;
        FinalPrice = TotalPrice - discountAmount;
    }

    public void ReleaseDiscountUsage(Discount discount)
    {
        if (!IsDiscountUsageConsumed)
            return;

        discount.RestoreUsage();
        IsDiscountUsageConsumed = false;
    }

    public CartModificationResult RefreshAfterCartChange(Discount? appliedDiscount, bool isCooperation)
    {
        RecalculateTotalPrice();

        if (OrderItems.Count == 0)
        {
            if (IsDiscountUsageConsumed && appliedDiscount is not null)
                ReleaseDiscountUsage(appliedDiscount);

            ClearDiscountApplication();
            TotalPrice = 0;
            FinalPrice = 0;
            return CartModificationResult.Empty();
        }

        if (!AppliedDiscountId.HasValue || appliedDiscount is null)
        {
            ClearDiscountApplication();
            FinalPrice = TotalPrice;
            return CartModificationResult.WithoutDiscount(TotalPrice);
        }

        if (IsDiscountUsageConsumed)
            ReleaseDiscountUsage(appliedDiscount);

        var evaluation = EvaluateDiscount(appliedDiscount, isCooperation);
        if (!evaluation.IsSuccess)
        {
            ClearDiscountApplication();
            FinalPrice = TotalPrice;
            return CartModificationResult.DiscountInvalidated(TotalPrice, evaluation.Failure!.Value);
        }

        AppliedDiscountId = appliedDiscount.Id;
        DiscountAmount = evaluation.DiscountAmount;
        FinalPrice = TotalPrice - evaluation.DiscountAmount;

        return CartModificationResult.WithDiscount(TotalPrice, evaluation.DiscountAmount);
    }

    public DiscountApplicationResult TryApplyDiscount(Discount discount, bool isCooperation)
    {
        if (IsDiscountUsageConsumed && AppliedDiscountId.HasValue && AppliedDiscountId != discount.Id)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.DiscountAlreadyConsumed);

        if (IsDiscountUsageConsumed && AppliedDiscountId == discount.Id)
        {
            var retryEvaluation = EvaluateDiscount(discount, isCooperation, isPaymentRetry: true);
            if (!retryEvaluation.IsSuccess)
                return retryEvaluation;

            DiscountAmount = retryEvaluation.DiscountAmount;
            FinalPrice = TotalPrice - retryEvaluation.DiscountAmount;
            return retryEvaluation;
        }

        var evaluation = EvaluateDiscount(discount, isCooperation);
        if (!evaluation.IsSuccess)
            return evaluation;

        AppliedDiscountId = discount.Id;
        DiscountAmount = evaluation.DiscountAmount;
        FinalPrice = TotalPrice - evaluation.DiscountAmount;
        return evaluation;
    }

    public DiscountApplicationResult EvaluateDiscount(
        Discount discount,
        bool isCooperation,
        bool isPaymentRetry = false)
    {
        if (!discount.IsActive)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.Inactive);

        if (discount.ExpiryDateOnUtc.HasValue && discount.ExpiryDateOnUtc.Value < DateTime.UtcNow)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.Expired);

        if (discount.IsCooperation && !isCooperation)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.CooperationOnly);

        if (!isPaymentRetry && discount.RemainingUses <= 0)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.UsageLimitReached);

        if (discount.UserId.HasValue && discount.UserId != UserId)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.UserNotEligible);

        if (discount.MinimumAmount.HasValue && TotalPrice < discount.MinimumAmount.Value)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.MinimumAmountNotMet);

        var eligibleItems = OrderItems.Where(item => IsItemEligibleForDiscount(item, discount)).ToList();
        if (eligibleItems.Count == 0)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.NoEligibleItems);

        decimal discountAmount;

        if (discount.Percentage > 0)
        {
            discountAmount = eligibleItems.Sum(item =>
            {
                var itemLineTotal = item.GetSumPrices() * item.Quantity;
                return itemLineTotal * discount.Percentage / 100m;
            });
        }
        else if (discount.Amount > 0)
        {
            discountAmount = discount.Amount;
        }
        else
        {
            return DiscountApplicationResult.Failed(DiscountValidationFailure.InvalidDiscountType);
        }

        if (discountAmount <= 0)
            return DiscountApplicationResult.Failed(DiscountValidationFailure.NoEligibleItems);

        if (discount.MaxDiscountAmount > 0 && discountAmount > discount.MaxDiscountAmount)
            discountAmount = discount.MaxDiscountAmount;

        discountAmount = Math.Min(discountAmount, TotalPrice);

        return DiscountApplicationResult.Successful(discountAmount);
    }

    public void ConsumeDiscountUsage(Discount discount)
    {
        if (IsDiscountUsageConsumed)
            return;

        discount.ConsumeUsage();
        IsDiscountUsageConsumed = true;
    }

    private static bool IsItemEligibleForDiscount(OrderItem item, Discount discount)
    {
        if (discount.ProductId.HasValue && item.ProductId != discount.ProductId.Value)
            return false;

        if (discount.SubCategoryId.HasValue && item.Product.SubCategoryId != discount.SubCategoryId.Value)
            return false;

        var circulationOrMeterOrCount = GetCirculationOrMeterOrCount(item);

        if (discount.FromCirculationOrMeterOrCount.HasValue && circulationOrMeterOrCount < discount.FromCirculationOrMeterOrCount.Value)
            return false;

        if (discount.ToCirculationOrMeterOrCount.HasValue && circulationOrMeterOrCount > discount.ToCirculationOrMeterOrCount.Value)
            return false;

        return true;
    }

    private static int GetCirculationOrMeterOrCount(OrderItem item)
    {
        var numericQuantities = item.OrderItemProperties
            .OfType<NumericOrderItemProperty>()
            .Select(property => property.Quantity);

        return numericQuantities.DefaultIfEmpty(item.Quantity).Max();
    }
}