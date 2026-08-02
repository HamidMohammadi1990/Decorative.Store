using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Product : BaseEntity
{
    public bool IsActive { get; private set; } = true;
    public string ProductCode { get; private set; } = default!;
    public decimal Price { get; private set; }
    public decimal? CompareAtPrice { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public int SubCategoryId { get; private set; }

    public SubCategory SubCategory { get; private set; } = default!;
    public ICollection<OrderItem> OrderItems { get; private set; } = default!;
    public ICollection<ProductFile> ProductFiles { get; private set; } = default!;
    public ICollection<Discount> ProductDiscounts { get; private set; } = default!;
    public ICollection<ProductComment> ProductComments { get; private set; } = default!;
    public ICollection<ProductFeature> ProductFeatures { get; private set; } = [];
    public ICollection<ProductProperty> ProductProperties { get; private set; } = default!;
    public ICollection<ProductDescription> ProductDescriptions { get; private set; } = default!;
    public ICollection<ProductOrderItemAttachmentType> ProductOrderItemAttachmentTypes { get; private set; } = default!;
    public ICollection<ProductTranslation> Translations { get; private set; } = [];

    public static Product Create(string productCode, int subCategoryId, decimal price, decimal? compareAtPrice = null)
        => new()
        {
            ProductCode = productCode,
            SubCategoryId = subCategoryId,
            Price = price,
            CompareAtPrice = compareAtPrice
        };

    public ProductTranslation UpsertTranslation(int languageId, string title, string slug, string description)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, slug, description);
            return existing;
        }

        var translation = ProductTranslation.Create(title, slug, description, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(
        bool isActive,
        string productCode,
        int subCategoryId,
        decimal price,
        decimal? compareAtPrice,
        int languageId,
        string title,
        string slug,
        string description)
    {
        IsActive = isActive;
        ProductCode = productCode;
        SubCategoryId = subCategoryId;
        Price = price;
        CompareAtPrice = compareAtPrice;
        UpsertTranslation(languageId, title, slug, description);
    }

    public void DeActive()
    {
        IsActive = false;
    }

    public void SetPricing(decimal price, decimal? compareAtPrice = null)
    {
        Price = price;
        CompareAtPrice = compareAtPrice;
    }

    public void AddFeature(ProductFeature productFeature)
    {
        ProductFeatures.Add(productFeature);
    }
}
