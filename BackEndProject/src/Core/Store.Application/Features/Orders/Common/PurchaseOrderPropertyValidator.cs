using Store.Common.Models;
using Store.Domain.Dtos.Products;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Common;

public static class PurchaseOrderPropertyValidator
{
    public static ErrorModel[] Validate(IReadOnlyList<ProductPropertyDto> productProperties, IReadOnlyList<OrderItemProperty> orderProperties)
    {
        var catalog = PurchaseOrderPropertyCatalog.Create(productProperties);

        if (orderProperties.Count == 0)
            return [];

        var errors = new List<ErrorModel>();
        var submittedPropertyIds = new HashSet<int>();

        void AddError(string code, string message) => errors.Add(ErrorModel.CreateLiteral(code, message));

        foreach (var orderProperty in orderProperties)
        {
            if (!submittedPropertyIds.Add(orderProperty.PropertyId))
            {
                AddError(orderProperty.PropertyId.ToString(), "داده های ارسالی نامعتبر است");
                continue;
            }

            if (!catalog.TryGetDefinition(orderProperty.PropertyId, out var definition))
            {
                AddError(orderProperty.PropertyId.ToString(), "شناسه نامعتبر می باشد");
                continue;
            }

            ValidateByType(orderProperty, definition, catalog, AddError);
        }

        return [.. errors];
    }

    private static void ValidateByType(
        OrderItemProperty orderProperty,
        PurchaseOrderPropertyCatalog.PropertyDefinition _,
        PurchaseOrderPropertyCatalog catalog,
        Action<string, string> addError)
    {
        switch (orderProperty)
        {
            case NumericOrderItemProperty { PropertyType: PropertyType.NumericWithItem } numericProperty:
                ValidateNumericWithItem(numericProperty, catalog, addError);
                break;
            case OrderItemProperty { PropertyType: PropertyType.Select } selectProperty:
                ValidateSelect(selectProperty, catalog, addError);
                break;
        }
    }

    private static void ValidateNumericWithItem(
        NumericOrderItemProperty numericProperty,
        PurchaseOrderPropertyCatalog catalog,
        Action<string, string> addError)
    {
        if (numericProperty.PropertyItemId is null or 0)
            return;

        if (!catalog.IsValidItemId(numericProperty.PropertyId, numericProperty.PropertyItemId.Value))
            addError(numericProperty.PropertyItemId.Value.ToString(), "شناسه نامعتبر می باشد");
    }

    private static void ValidateSelect(
        OrderItemProperty selectProperty,
        PurchaseOrderPropertyCatalog catalog,
        Action<string, string> addError)
    {
        if (selectProperty.PropertyItemId is null or 0)
            return;

        if (!catalog.IsValidItemId(selectProperty.PropertyId, selectProperty.PropertyItemId.Value))
            addError(selectProperty.PropertyItemId.Value.ToString(), "شناسه نامعتبر می باشد");
    }
}
