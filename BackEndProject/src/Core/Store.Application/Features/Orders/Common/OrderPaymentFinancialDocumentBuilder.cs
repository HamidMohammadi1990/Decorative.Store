using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Orders.Common;

public static class OrderPaymentFinancialDocumentBuilder
{
    public static FinancialDocument Build(
        Order order,
        OrderPaymentSlice paymentSlice,
        int financialYearId,
        bool isFullyPaid)
    {
        var trackingDescription = $"سفارش با کد پیگیری {order.TrackingCode}";

        var financialDocument = FinancialDocument.Create(
            order.Id,
            FinancialDocumentType.BankTransaction,
            trackingDescription,
            string.Empty,
            financialYearId);

        var withoutVatPrice = paymentSlice.FinalAmount - paymentSlice.VatAmount;

        financialDocument.AddDetail(FinancialDocumentDetail.Create(
            AccountPartyType.Customer,
            OrderPaymentConstants.CustomerChartOfAccountId,
            financialDocument.Id,
            paymentSlice.FinalAmount,
            0m,
            $"بدهکار شدن مشتری بابت {trackingDescription}"));

        financialDocument.AddDetail(FinancialDocumentDetail.Create(
            AccountPartyType.Intermediary,
            OrderPaymentConstants.IntermediaryChartOfAccountId,
            financialDocument.Id,
            0m,
            withoutVatPrice,
            $"بستانکار شدن واسط بابت {trackingDescription}"));

        financialDocument.AddDetail(FinancialDocumentDetail.Create(
            AccountPartyType.Vat,
            OrderPaymentConstants.VatChartOfAccountId,
            financialDocument.Id,
            0m,
            paymentSlice.VatAmount,
            $"بستانکار شدن مالیات بابت {trackingDescription}"));

        if (paymentSlice.VatAmount > 0)
        {
            var orderVat = OrderVat.Create(
                order.Id,
                financialDocument.Id,
                paymentSlice.VatAmount,
                isFullyPaid ? OrderVatStatusType.Paid : OrderVatStatusType.Pending);

            financialDocument.OrderVats.Add(orderVat);
            order.OrderVats.Add(orderVat);
        }

        return financialDocument;
    }
}
