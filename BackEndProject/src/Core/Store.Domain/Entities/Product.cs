using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public string Description { get; private set; } = default!;
    public string ProductCode { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public int SubCategoryId { get; private set; }


    public SubCategory SubCategory { get; private set; } = default!;
    public ICollection<OrderItem> OrderItems { get; private set; } = default!;
    public ICollection<ProductFile> ProductFiles { get; private set; } = default!;
    public ICollection<Discount> ProductDiscounts { get; private set; } = default!;
    public ICollection<ProductPrice> ProductPrices { get; private set; } = default!;
    public ICollection<CompanyProduct> CompanyProducts { get; private set; } = default!;
    public ICollection<ProductComment> ProductComments { get; private set; } = default!;
    public ICollection<ProductFeature> ProductFeatures { get; private set; } = [];
    public ICollection<ProductProperty> ProductProperties { get; private set; } = default!;
    public ICollection<ProductDescription> ProductDescriptions { get; private set; } = default!;
    public ICollection<ProductOrderItemAttachmentType> ProductOrderItemAttachmentTypes { get; private set; } = default!;


    public static Product Create(string title, string slug, string description, string productCode, int subCategoryId)
        => new()
        {
            Title = title,
            Slug = slug,
            Description = description,
            ProductCode = productCode,
            SubCategoryId = subCategoryId
        };
    public void Update(string title, string slug, bool isActive, string description, string productCode, int subCategoryId)
    {
        Title = title;
        Slug = slug;
        IsActive = isActive;
        Description = description;
        ProductCode = productCode;
        SubCategoryId = subCategoryId;
    }
    public void DeActive()
    {
        IsActive = false;
    }
    public void AddFeature(ProductFeature productFeature)
    {
        ProductFeatures.Add(productFeature);
    }
}