namespace Store.ProductImport.Models;

public sealed class ParsedInventoryProduct
{
    public int Row { get; init; }
    public string Barcode { get; init; } = string.Empty;
    public string TitleFa { get; set; } = string.Empty;
    public string ExternalCode { get; init; } = string.Empty;
    public string PackQuantity { get; init; } = string.Empty;
    public string PackUnitFa { get; init; } = string.Empty;
    public string MeasurementFa { get; init; } = string.Empty;
    public decimal PriceToman { get; init; }
    public string ProductCode { get; set; } = string.Empty;
    public string SlugFa { get; set; } = string.Empty;
    public string SlugEn { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string DescriptionFa { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string SubCategorySlug { get; set; } = "kitchen-dining/glassware";
    public byte[]? ImageBytes { get; set; }
    public string? ImageExtension { get; set; }
}
