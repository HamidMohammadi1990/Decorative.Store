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
        {
            return catalog.MandatoryPropertyIds.Count == 0
                ? []
                : [ErrorModel.CreateLiteral("General", "ارسال مقادیر ستاره دار الزامی است")];
        }

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

            ValidateMandatoryValue(orderProperty, definition, AddError);
            ValidateByType(orderProperty, definition, catalog, AddError);
        }

        foreach (var mandatoryPropertyId in catalog.MandatoryPropertyIds)
        {
            if (!submittedPropertyIds.Contains(mandatoryPropertyId))
                AddError(mandatoryPropertyId.ToString(), "ارسال این مقدار الزامی است");
        }

        return [.. errors];
    }

    private static void ValidateMandatoryValue(
        OrderItemProperty orderProperty,
        PurchaseOrderPropertyCatalog.PropertyDefinition definition,
        Action<string, string> addError)
    {
        if (!definition.IsMandatory)
            return;

        switch (orderProperty)
        {
            case BooleanOrderItemProperty booleanProperty when !booleanProperty.IsSelected:
                addError(orderProperty.PropertyId.ToString(), "انتخاب این گزینه الزامی است");
                break;
            case TextOrderItemProperty textProperty when string.IsNullOrWhiteSpace(textProperty.Value):
                addError(orderProperty.PropertyId.ToString(), "وارد کردن مقدار الزامی است");
                break;
            case OrderItemProperty { PropertyType: PropertyType.Select } selectProperty
                when selectProperty.PropertyItemId is null or 0:
                addError(orderProperty.PropertyId.ToString(), "انتخاب آیتم الزامی است");
                break;
            case NumericOrderItemProperty { PropertyType: PropertyType.NumericWithItem } numericProperty
                when numericProperty.PropertyItemId is null or 0:
                addError(orderProperty.PropertyId.ToString(), "انتخاب آیتم الزامی است");
                break;
        }
    }

    private static void ValidateByType(
        OrderItemProperty orderProperty,
        PurchaseOrderPropertyCatalog.PropertyDefinition definition,
        PurchaseOrderPropertyCatalog catalog,
        Action<string, string> addError)
    {
        if (definition.RulePropertyType is null)
            return;

        switch (orderProperty)
        {
            case NumericOrderItemProperty numericProperty
                when definition.RulePropertyType is PropertyType.Numeric or PropertyType.NumericWithItem:
                ValidateNumeric(numericProperty, definition, catalog, addError);
                break;
            case DimensionsOrderItemProperty dimensionsProperty
                when definition.RulePropertyType is PropertyType.Dimensions:
                ValidateDimensions(dimensionsProperty, definition, addError);
                break;
            case TextOrderItemProperty textProperty when definition.RulePropertyType is PropertyType.Text:
                ValidateText(textProperty, definition, addError);
                break;
            case OrderItemProperty { PropertyType: PropertyType.Select } selectProperty:
                ValidateSelect(selectProperty, catalog, addError);
                break;
        }
    }

    private static void ValidateNumeric(
        NumericOrderItemProperty numericProperty,
        PurchaseOrderPropertyCatalog.PropertyDefinition definition,
        PurchaseOrderPropertyCatalog catalog,
        Action<string, string> addError)
    {
        if (definition.MinQuantity is decimal minQuantity && numericProperty.Quantity < minQuantity)
            addError(numericProperty.PropertyId.ToString(), $"حداقل مقدار {minQuantity}");

        if (definition.MaxQuantity is decimal maxQuantity && numericProperty.Quantity > maxQuantity)
            addError(numericProperty.PropertyId.ToString(), $"حداکثر مقدار {maxQuantity}");

        if (numericProperty.PropertyType is not PropertyType.NumericWithItem)
            return;

        if (numericProperty.PropertyItemId is null or 0)
            return;

        if (!catalog.IsValidItemId(numericProperty.PropertyId, numericProperty.PropertyItemId.Value))
            addError(numericProperty.PropertyItemId.Value.ToString(), "شناسه نامعتبر می باشد");
    }

    private static void ValidateDimensions(
        DimensionsOrderItemProperty dimensionsProperty,
        PurchaseOrderPropertyCatalog.PropertyDefinition definition,
        Action<string, string> addError)
    {
        if (definition.MinHeight is decimal minHeight && dimensionsProperty.Height < minHeight)
            addError(dimensionsProperty.PropertyId.ToString(), $"حداقل مقدار {minHeight}");

        if (definition.MaxHeight is decimal maxHeight && dimensionsProperty.Height > maxHeight)
            addError(dimensionsProperty.PropertyId.ToString(), $"حداکثر مقدار {maxHeight}");

        if (definition.MinWidth is decimal minWidth && dimensionsProperty.Width < minWidth)
            addError(dimensionsProperty.PropertyId.ToString(), $"حداقل مقدار {minWidth}");

        if (definition.MaxWidth is decimal maxWidth && dimensionsProperty.Width > maxWidth)
            addError(dimensionsProperty.PropertyId.ToString(), $"حداکثر مقدار {maxWidth}");
    }

    private static void ValidateText(
        TextOrderItemProperty textProperty,
        PurchaseOrderPropertyCatalog.PropertyDefinition definition,
        Action<string, string> addError)
    {
        var length = textProperty.Value?.Length ?? 0;

        if (definition.MinLength is int minLength && length < minLength)
            addError(textProperty.PropertyId.ToString(), $"حداقل طول {minLength}");

        if (definition.MaxLength is int maxLength && length > maxLength)
            addError(textProperty.PropertyId.ToString(), $"حداکثر طول {maxLength}");
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
