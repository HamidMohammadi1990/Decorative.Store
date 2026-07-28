using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class ProductFeatureType : BaseEntity
{
    public string Name { get; private set; } = default!;
    public ProductFeatureTypeCode Type { get; set; }
    public ProductFeatureDataType DataType { get; set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;


    public ICollection<ProductFeature> ProductFeatures { get; private set; } = default!;


    public static ProductFeatureType Create(string name, ProductFeatureTypeCode type, ProductFeatureDataType dataType, string? description, bool isActive)
        => new()
        {
            Name = name,
            Type = type,
            DataType = dataType,
            Description = description,
            IsActive = isActive
        };

    public void Update(string name, ProductFeatureTypeCode type, ProductFeatureDataType dataType, string? description, bool isActive)
    {
        Name = name;
        Type = type;
        DataType = dataType;
        Description = description;
        IsActive = isActive;
    }
}