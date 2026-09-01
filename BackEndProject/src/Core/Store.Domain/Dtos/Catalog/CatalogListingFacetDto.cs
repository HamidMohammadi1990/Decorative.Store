namespace Store.Domain.Dtos.Catalog;

public record CatalogListingFacetOptionDto(
    string Value,
    string Label,
    int Count,
    string? Swatch = null);

public record CatalogListingPriceRangeDto(
    decimal Min,
    decimal Max,
    decimal Step,
    decimal? SelectedMin,
    decimal? SelectedMax);

public record CatalogListingFacetGroupDto(
    string Id,
    string Label,
    string Type,
    List<CatalogListingFacetOptionDto> Options,
    CatalogListingPriceRangeDto? Range = null);
