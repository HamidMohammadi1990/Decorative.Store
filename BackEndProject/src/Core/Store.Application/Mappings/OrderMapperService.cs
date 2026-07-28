using Edition.Application.Common.Utilities;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Orders.Queries;
using Edition.Application.Features.Orders.Commands;
using Edition.Application.Features.Orders.Common;
using Store.Common.Extensions;
using Store.Domain.Dtos.Products;
using Store.Domain.Enums;
using Store.Domain.Dtos.UserAddresses;
using Store.Domain.Dtos.Others;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Orders;
using Store.Domain.Dtos.ProductPropertyRules;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class OrderMapperService : IOrderMapperService
{
    public GetAllOrderRequestDto Map(GetAllOrderRequest model)
    {
        return new GetAllOrderRequestDto
        {
            Title = model.Title,
            UserId = model.UserId,
            Status = model.Status,
            IsFinaly = model.IsFinaly,
            Pagination = model.Pagination,
            TrackingCode = model.TrackingCode
        }.WithContentPolicy<Order, GetAllOrderRequestDto>(model);
    }

    public CheckoutOrderResponse Map(List<ProductPropertyDto> properties, List<DeliveryType> deliveryTypes,
         List<PostType> postTypes, List<UserAddressSummaryDto> userAddresses,
         ProductSummaryDto productDetail, List<ProductOrderAttachmentDto> orderAttachments, bool userIsCooperation)
    {
        var checkoutProduct = new CheckoutProductResponse
        {
            Id = productDetail.Id,
            Slug = productDetail.Slug,
            Title = productDetail.Title,
            Description = productDetail.Description,
            ProductCode = productDetail.ProductCode,
            SubCategoryTitle = productDetail.SubCategoryTitle,
            Images = [.. productDetail.Images.Select(x => new CheckoutProductImageResponse
            {
                Title = x.Title,
                Url = x.Url
            })],
        };

        var checkoutProperties = ToCheckoutProperties(properties, userIsCooperation);

        var checkoutDeliveryTypes =
            deliveryTypes
            .Select(x => new CheckoutDeliveryTypeResponse
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();

        var checkoutPostTypes =
            postTypes
            .Select(x => new CheckoutPostTypeResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description ?? ""
            }).ToList();

        var checkoutUserAddresses =
            userAddresses
            .Select(x => new CheckoutUserAddressResponse
            {
                Id = x.Id,
                Title = x.Title,
                Address = x.Address
            }).ToList();

        var attachments = orderAttachments
            .Select(x => new CheckoutOrderAttachmentResponse
            {
                Id = x.ProductOrderItemAttachmentTypeId,
                Title = x.Title,
                Description = x.Description ?? "",
                Restriction = new CheckoutOrderAttachmentRestrictionResponse
                {
                    MinWidth = x.MinWidth,
                    MaxWidth = x.MaxWidth,
                    MinHeight = x.MinHeight,
                    MaxHeight = x.MaxHeight,
                    ColorMode = x.ColorMode,
                    IsRequired = x.IsRequired,
                    MaxFileSizeInMegaBytes = FileSizeConverter.Convert(x.MaxFileSizeInBytes, FileSizeUnitType.Megabytes),
                    MinVerticalResolution = x.MinVerticalResolution,
                    MaxVerticalResolution = x.MaxVerticalResolution,
                    MinHorizontalResolution = x.MinHorizontalResolution,
                    MaxHorizontalResolution = x.MaxHorizontalResolution
                }
            }).ToList();

        return new CheckoutOrderResponse
        {
            Product = checkoutProduct,
            Properties = checkoutProperties,
            DeliveryTypes = checkoutDeliveryTypes,
            PostTypes = checkoutPostTypes,
            Addresses = checkoutUserAddresses,
            Attachments = attachments,
        };
    }

    public PagedResult<GetAllOrderResponse> Map(PagedResult<GetAllOrderDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllOrderResponse
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                UserId = x.UserId,
                VatPrice = x.VatPrice,
                IsFinaly = x.IsFinaly,
                FinalPrice = x.FinalPrice,
                TotalPrice = x.TotalPrice,
                TrackingCode = x.TrackingCode,
                CreatedOnUtc = x.CreatedOnUtc,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                TotalCommissionPrice = x.TotalCommissionPrice
            })
            .ToList();

        return PagedResult<GetAllOrderResponse>.Create(items, model);
    }

    public GetOrderResponse Map(Order model)
    {
        return new GetOrderResponse
        {
            Id = model.Id,
            Title = model.Title,
            UserId = model.UserId,
            VatPrice = model.VatPrice,
            IsFinaly = model.IsFinaly,
            Status = model.Status,
            FinalPrice = model.FinalPrice,
            TotalPrice = model.TotalPrice,
            TrackingCode = model.TrackingCode,
            CreatedOnUtc = model.CreatedOnUtc,
            TotalCommissionPrice = model.TotalCommissionPrice
        };
    }

    private static List<CheckoutPropertiesResponse> ToCheckoutProperties(List<ProductPropertyDto> productProperties, bool userIsCooperation)
    {
        var result =
             productProperties
            .GroupBy(x => x.CategoryTitle)
            .Select(x => new CheckoutPropertiesResponse
            {
                CategoryTitle = x.Key,
                Properties = x.DistinctBy(x => x.PropertyId).Select(p =>
                {
                    var propertyRuleItem = productProperties.FirstOrDefault(rule => p.ProductPropertyId == rule.PropertyRuleProductPropertyId);
                    var propertyRule = ToPropertyRule(propertyRuleItem, false);
                    var property = new CheckoutPropertyResponse
                    {
                        Id = p.PropertyId,
                        Title = p.PropertyTitle,
                        PropertyType = p.PropertyType,
                        Rule = propertyRule,
                        Items = [],
                        Parents = [],
                        Price = PriceField.Create((!userIsCooperation ? p.PropertyPrice : p.PropertyCooperationPrice) ?? 0),
                        Dependencies = [],
                        DependentPropertyId = productProperties.FirstOrDefault(x => x.DependencyParentPropertyItemId == p.PropertyId)?.PropertyId,
                    };
                    property.Items =
                            productProperties
                            .Where(x => x.PropertyItemPropertyId == p.PropertyId)
                            .Select(x => new CheckoutPropertyItemResponse
                            {
                                Id = x.PropertyItemId!.Value,
                                Title = x.PropertyItemTitle!,
                                Price = PriceField.Create((!userIsCooperation ? x.PropertyItemPrice : x.PropertyItemCooperationPrice) ?? 0)
                            }).ToList();
                    property.Dependencies =
                            productProperties
                            .Where(d => property.Items.Any(i => i.Id == d.DependencyDependentPropertyItemId))
                            .Select(d => new CheckoutPropertItemDependencyResponse
                            {
                                ParentId = d.DependencyParentPropertyItemId!.Value,
                                DependentId = d.DependencyDependentPropertyItemId!.Value
                            }).ToList();
                    property.Parents =
                            productProperties
                            .Where(item => p.PropertyParentId is not null && p.PropertyParentId == item.ParentPropertyId)
                            .Select(parentProperty =>
                            {
                                var parentPropertyRuleItem =
                                        productProperties
                                       .FirstOrDefault(rule => parentProperty.ParentProductPropertyId == rule.ParentPropertyRuleProductPropertyId);
                                var parentPropertyRule = ToPropertyRule(parentPropertyRuleItem, true);
                                var property = new CheckoutPropertyResponse
                                {
                                    Id = parentProperty.ParentPropertyId ?? 0,
                                    Title = parentProperty.ParentPropertyTitle ?? "",
                                    PropertyType = parentProperty.ParentPropertyType ?? 0,
                                    Rule = parentPropertyRule,
                                    Items = [],
                                    Parents = [],
                                    Dependencies = [],
                                    Price = PriceField.Create((!userIsCooperation ? parentProperty.ParentPropertyPrice : parentProperty.ParentPropertyCooperationPrice) ?? 0),
                                };

                                if (parentProperty.ParentPropertyId is not null)
                                {
                                    property.Items =
                                         productProperties
                                        .Where(x => x.ParentPropertyItemPropertyId == parentProperty.ParentPropertyId)
                                        .Select(x => new CheckoutPropertyItemResponse
                                        {
                                            Id = x.ParentPropertyItemId!.Value,
                                            Title = x.ParentPropertyItemTitle!,
                                            Price = PriceField.Create((!userIsCooperation ? x.ParentPropertyItemPrice : x.ParentPropertyItemCooperationPrice) ?? 0)
                                        }).ToList();
                                }

                                return property;
                            }).ToList();

                    return property;
                }).ToList()
            }).ToList();

        return result;

        static ProductPropertyRuleDto? ToPropertyRule(ProductPropertyDto? productProperty, bool isParent)
        {
            if (productProperty is null || productProperty.PropertyRulePropertyType is null)
                return null;

            return productProperty.PropertyRulePropertyType switch
            {
                PropertyType.Numeric or PropertyType.NumericWithItem => new NumericProductPropertyRuleDto
                {
                    IsMandatory = !isParent ? productProperty.PropertyRuleIsMandatory!.Value : productProperty.ParentPropertyRuleIsMandatory!.Value,
                    Description = !isParent ? productProperty.PropertyRuleDescription! : productProperty.ParentPropertyRuleDescription!,
                    MinQuantity = !isParent ? productProperty.PropertyRuleMinQuantity!.Value : productProperty.ParentPropertyRuleMinQuantity!.Value,
                    MaxQuantity = !isParent ? productProperty.PropertyRuleMaxQuantity!.Value : productProperty.ParentPropertyRuleMaxQuantity!.Value
                },
                PropertyType.Dimensions => new DimensionsProductPropertyRuleDto
                {
                    IsMandatory = !isParent ? productProperty.PropertyRuleIsMandatory!.Value : productProperty.ParentPropertyRuleIsMandatory!.Value,
                    Description = !isParent ? productProperty.PropertyRuleDescription : productProperty.ParentPropertyRuleDescription,
                    MinWidth = !isParent ? productProperty.PropertyRuleMinWidth!.Value : productProperty.ParentPropertyRuleMinWidth!.Value,
                    MaxWidth = !isParent ? productProperty.PropertyRuleMaxWidth!.Value : productProperty.ParentPropertyRuleMaxWidth!.Value,
                    MinHeight = !isParent ? productProperty.PropertyRuleMinHeight!.Value : productProperty.ParentPropertyRuleMinHeight!.Value,
                    MaxHeight = !isParent ? productProperty.PropertyRuleMaxHeight!.Value : productProperty.ParentPropertyRuleMaxHeight!.Value
                },
                PropertyType.Text => new TextProductPropertyRuleDto
                {
                    IsMandatory = !isParent ? productProperty.PropertyRuleIsMandatory!.Value : productProperty.ParentPropertyRuleIsMandatory!.Value,
                    Description = !isParent ? productProperty.PropertyRuleDescription : productProperty.ParentPropertyRuleDescription,
                    MinLength = !isParent ? productProperty.PropertyRuleMinLength!.Value : productProperty.ParentPropertyRuleMinLength!.Value,
                    MaxLength = !isParent ? productProperty.PropertyRuleMaxLength!.Value : productProperty.ParentPropertyRuleMaxLength!.Value
                },
                _ => new ProductPropertyRuleDto
                {
                    IsMandatory = !isParent ? productProperty.PropertyRuleIsMandatory!.Value : productProperty.ParentPropertyRuleIsMandatory!.Value,
                    Description = !isParent ? productProperty.PropertyRuleDescription : productProperty.ParentPropertyRuleDescription
                }
            };
        }
    }

    public List<OrderItemProperty> ToOrderItemProperties(List<BaseOrderProperty> properties, List<ProductPropertyDto> productProperties, bool userIsCooperation)
    {
        static BooleanOrderItemProperty ToBooleanProperty(BooleanOrderProperty property)
           => BooleanOrderItemProperty.Create(property.Id, property.IsSelected, property.PropertyType);

        static NumericOrderItemProperty ToNumericProperty(NumericOrderProperty property)
            => NumericOrderItemProperty.Create(property.Id, property.Quantity, property.PropertyType);

        static NumericOrderItemProperty ToNumericWithItemProperty(NumericWithItemOrderProperty property)
            => NumericOrderItemProperty.Create(property.Id, property.ItemId, property.Quantity, property.PropertyType);

        static DimensionsOrderItemProperty ToDimensionsProperty(DimensionsOrderProperty property)
            => DimensionsOrderItemProperty.Create(property.Id, property.Width, property.Height, property.PropertyType);

        static OrderItemProperty ToSelectProperty(SelectOrderProperty property)
            => OrderItemProperty.Create(property.Id, property.ItemId, property.PropertyType);

        static TextOrderItemProperty ToTextProperty(TextOrderProperty property)
            => TextOrderItemProperty.Create(property.Id, property.PropertyType, property.Value);

        var flatProperties = FlattenOrderProperties(properties).ToList();
        var orderProperties = new List<OrderItemProperty>(flatProperties.Count);

        foreach (var property in flatProperties)
        {
            switch (property.PropertyType)
            {
                case PropertyType.Boolean:
                    orderProperties.Add(ToBooleanProperty((BooleanOrderProperty)property));
                    break;
                case PropertyType.Numeric:
                    orderProperties.Add(ToNumericProperty((NumericOrderProperty)property));
                    break;
                case PropertyType.NumericWithItem:
                    orderProperties.Add(ToNumericWithItemProperty((NumericWithItemOrderProperty)property));
                    break;
                case PropertyType.Dimensions:
                    orderProperties.Add(ToDimensionsProperty((DimensionsOrderProperty)property));
                    break;
                case PropertyType.Select:
                    orderProperties.Add(ToSelectProperty((SelectOrderProperty)property));
                    break;
                case PropertyType.Text:
                    orderProperties.Add(ToTextProperty((TextOrderProperty)property));
                    break;
            }
        }

        var catalog = PurchaseOrderPropertyCatalog.Create(productProperties);

        foreach (var orderProperty in orderProperties)
        {
            if (!catalog.TryGetDefinition(orderProperty.PropertyId, out var definition))
                continue;

            var propertyPrice = definition.PropertyPrice(userIsCooperation);
            if (propertyPrice.HasValue)
                orderProperty.SetPropertyPrice(propertyPrice.Value);

            if (orderProperty.PropertyType != PropertyType.NumericWithItem || orderProperty.PropertyItemId is not int itemId)
                continue;

            var itemRow = catalog.FindItemRow(orderProperty.PropertyId, itemId);
            if (itemRow is null)
                continue;

            var itemPrice = definition.PropertyItemPrice(userIsCooperation, itemRow);
            if (itemPrice.HasValue)
                orderProperty.SetPropertyItemPrice(itemPrice.Value);
        }

        return orderProperties;
    }

    private static IEnumerable<BaseOrderProperty> FlattenOrderProperties(IEnumerable<BaseOrderProperty> properties)
    {
        foreach (var property in properties)
        {
            if (property is HasParentsOrderProperty { Properties: { } nestedProperties })
            {
                foreach (var nestedProperty in FlattenOrderProperties(nestedProperties))
                    yield return nestedProperty;

                continue;
            }

            yield return property;
        }
    }

    public List<GetStatusSummaryOrderResponse> MapToStatusSummary(List<GetStatusSummaryPropertiesDto> orders)
    {
        return orders
            .Select(x => new GetStatusSummaryOrderResponse
            {
                Id = (int)x.Status,
                Count = x.Count,
                Title = x.Status.ToDisplay()
            })
            .ToList();
    }

    public List<GetOrderByStatusResponse> MapToUserOrdersByStatus(List<GetUserOrdersByStatusDto> orders)
    {
        return [.. orders
            .GroupBy(o => o.OrderId)
            .Select(g =>
            {
                var order = g.First();
                return new GetOrderByStatusResponse
                {
                    Id = order.OrderId,
                    Title = order.Title,
                    FinalPrice = order.FinalPrice,
                    TrackingCode = order.TrackingCode,
                    CreatedOnUtc = order.CreatedOnUtc,
                    Items = [.. g.Select(i => new GetOrderByStatusItemResponse
                    {
                        Id = i.OrderItemId,
                        Quantity = i.ItemQuantity,
                        ProductImage = i.ProductImage
                    })]
                };
            })];
    }

    public GetOrderDetailResponse? MapToOrderDetail(OrderDetailDto? order)
    {
        if (order is null) return null!;

        return new GetOrderDetailResponse
        {
            OrderId = order.Id,
            Title = order.Title,
            Status = order.Status,
            IsFinaly = order.IsFinaly,
            TotalPrice = order.TotalPrice,
            FinalPrice = order.FinalPrice,
            CreatedOnUtc = order.CreatedOnUtc,
            TrackingCode = order.TrackingCode,
            Items = [.. order.Items.Select(item => new OrderItemResponse
            {
                Id = item.Id,
                Status = item.Status,
                Quantity = item.Quantity,
                PostType = item.PostTypeTitle,
                Description = item.Description,
                CreatedOnUtc = item.CreatedOnUtc,
                ProductPrice = item.ProductPrice,
                IsNeedToDesign = item.IsNeedToDesign,
                DeliveryType = item.DeliveryTypeTitle,
                EmergencyPhoneNumber = item.EmergencyPhoneNumber,
                Product = new OrderItemProductSummaryResponse
                {
                    Id = item.Product.Id,
                    Slug = item.Product.Slug,
                    Title = item.Product.Title,
                    ProductCode = item.Product.ProductCode
                },
                Company = new OrderItemCompanySummaryResponse
                {
                    Id = item.Company.Id,
                    Name = item.Company.Name,
                    Code = item.Company.Code,
                    PhoneNumber = item.Company.PhoneNumber
                },
                UserAddress = new OrderItemUserAddressResponse
                {
                    Title = item.UserAddress.Title,
                    CityTitle = item.UserAddress.CityTitle,
                    RecipientFirstName = item.UserAddress.RecipientFirstName,
                    RecipientLastName = item.UserAddress.RecipientLastName,
                    Address = item.UserAddress.Address,
                    PostalCode = item.UserAddress.PostalCode,
                    PhoneNumber = item.UserAddress.PhoneNumber
                },
                Attachments = [.. item.Attachments.Select(att => new OrderItemAttachmentResponse
                {
                    FileName = att.FileName,
                    TypeTitle = att.TypeTitle
                })],
                Properties = [.. item.Properties.Select(MapOrderItemProperty)]
            })]
        };
        static OrderItemPropertyResponse MapOrderItemProperty(OrderItemPropertyDetailDto property)
        {
            if (property is null) return null!;

            return property.PropertyType switch
            {
                PropertyType.Boolean => new BooleanOrderItemPropertyResponse
                {
                    Title = property.Title,
                    Price = property.Price ?? 0,
                    IsSelected = property.IsSelected!.Value,
                    PropertyType = property.PropertyType,
                },
                PropertyType.Numeric => new NumericOrderItemPropertyResponse
                {
                    Title = property.Title,
                    Price = property.Price ?? 0,
                    Quantity = property.Quantity ?? 0,
                    PropertyType = property.PropertyType
                },
                PropertyType.NumericWithItem => new NumericWithItemOrderItemPropertyResponse
                {
                    Title = property.Title,
                    Price = property.Price ?? 0,
                    Quantity = property.Quantity ?? 0,
                    ItemTitle = property.ItemTitle,
                    ItemPrice = property.ItemPrice,
                    PropertyType = property.PropertyType
                },
                PropertyType.Select => new SelectOrderItemPropertyResponse
                {
                    Title = property.Title,
                    Price = property.Price ?? 0,
                    ItemTitle = property.ItemTitle,
                    ItemPrice = property.ItemPrice,
                    PropertyType = property.PropertyType
                },
                PropertyType.Dimensions => new DimensionsOrderItemPropertyResponse
                {
                    Title = property.Title,
                    Width = property.Width ?? 0,
                    Price = property.Price ?? 0,
                    Height = property.Height ?? 0,
                    PropertyType = property.PropertyType,
                },
                PropertyType.Text => new TextOrderItemPropertyResponse
                {
                    Title = property.Title,
                    Price = property.Price ?? 0,
                    Value = property.Value,
                    PropertyType = property.PropertyType
                },
                _ => new OrderItemPropertyResponse
                {
                    Title = property.Title,
                    Price = property.Price ?? 0,
                    PropertyType = property.PropertyType
                }
            };
        }
    }
}