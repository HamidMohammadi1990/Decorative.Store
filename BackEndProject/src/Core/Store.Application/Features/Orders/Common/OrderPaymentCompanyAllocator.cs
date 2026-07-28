using Store.Domain.Entities;

namespace Edition.Application.Features.Orders.Common;

public static class OrderPaymentCompanyAllocator
{
    public static IReadOnlyList<OrderCompanyPaymentSlice> Allocate(Order order, decimal vatRate)
    {
        var grossByCompany = order.OrderItems
            .GroupBy(item => item.CompanyId)
            .Select(group => new
            {
                CompanyId = group.Key,
                GrossAmount = group.Sum(item => item.GetSumPrices() * item.Quantity)
            })
            .OrderBy(entry => entry.CompanyId)
            .ToList();

        if (grossByCompany.Count == 0)
            return [];

        var totalGross = grossByCompany.Sum(entry => entry.GrossAmount);
        if (totalGross <= 0)
            return [];

        var slices = new List<OrderCompanyPaymentSlice>(grossByCompany.Count);
        var allocatedDiscount = 0m;
        var allocatedVat = 0m;
        var allocatedFinal = 0m;

        for (var index = 0; index < grossByCompany.Count; index++)
        {
            var entry = grossByCompany[index];
            var isLast = index == grossByCompany.Count - 1;

            var discountShare = isLast
                ? order.DiscountAmount - allocatedDiscount
                : ProportionalShare(order.DiscountAmount, entry.GrossAmount, totalGross);

            var netAmount = Math.Max(0m, entry.GrossAmount - discountShare);
            var vatAmount = isLast
                ? order.VatPrice - allocatedVat
                : Math.Round(netAmount * vatRate, 2, MidpointRounding.AwayFromZero);

            var finalAmount = isLast
                ? order.FinalPrice - allocatedFinal
                : netAmount + vatAmount;

            allocatedDiscount += discountShare;
            allocatedVat += vatAmount;
            allocatedFinal += finalAmount;

            slices.Add(new OrderCompanyPaymentSlice(
                entry.CompanyId,
                entry.GrossAmount,
                discountShare,
                netAmount,
                vatAmount,
                finalAmount));
        }

        return slices;
    }

    public static IReadOnlyList<decimal> DistributeAmount(decimal totalAmount, IReadOnlyList<decimal> weights)
    {
        if (weights.Count == 0)
            return [];

        if (totalAmount == 0)
            return Enumerable.Repeat(0m, weights.Count).ToList();

        var weightSum = weights.Sum();
        if (weightSum <= 0)
            return Enumerable.Repeat(0m, weights.Count).ToList();

        var shares = new decimal[weights.Count];
        var allocated = 0m;

        for (var index = 0; index < weights.Count; index++)
        {
            var isLast = index == weights.Count - 1;
            shares[index] = isLast
                ? totalAmount - allocated
                : ProportionalShare(totalAmount, weights[index], weightSum);

            allocated += shares[index];
        }

        return shares;
    }

    private static decimal ProportionalShare(decimal total, decimal part, decimal whole)
        => Math.Round(total * part / whole, 2, MidpointRounding.AwayFromZero);
}
