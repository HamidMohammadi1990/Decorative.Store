namespace Store.Domain.Dtos.Catalog;

public class CatalogListingFacetLabelLookupDto
{
    public Dictionary<string, string> GroupLabels { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, Dictionary<string, string>> OptionLabels { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);
}
