using Store.Domain.Entities;

namespace Store.Domain.Enums;

public class ProductFeature
{
    public int Id { get; private set; }
    public int ProductId { get; set; }
    public int ProductFeatureTypeId { get; private set; }
    public string Value { get; private set; } = default!;


    public Product Product { get; private set; } = default!;
    public ProductFeatureType ProductFeatureType { get; private set; } = default!;


    public static ProductFeature Create(int productId, int productFeatureTypeId, string value)
        => new()
        {
            Value = value,
            ProductId = productId,
            ProductFeatureTypeId = productFeatureTypeId
        };
}