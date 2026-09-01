using Edition.Application.Features.Catalog.Queries;
using Store.Domain.Dtos.Catalog;

namespace Edition.Application.Features.Catalog.Services;

public static class CatalogListingFilterProcessor
{
    private static readonly TimeSpan NewProductWindow = TimeSpan.FromDays(90);

    private static readonly (string Id, decimal Min, decimal Max)[] PriceBuckets =
    [
        ("under-5m", 0, 4_999_999),
        ("5m-10m", 5_000_000, 9_999_999),
        ("10m-15m", 10_000_000, 14_999_999),
        ("15m-20m", 15_000_000, 19_999_999),
        ("over-20m", 20_000_000, decimal.MaxValue),
    ];

    private static readonly HashSet<string> ReservedQueryKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "path", "minPrice", "maxPrice", "inStock", "onSale", "isNew", "minRating",
        "sort", "page", "pageSize", "price",
    };

    public static bool IsReservedQueryKey(string key)
        => ReservedQueryKeys.Contains(key);

    private static readonly Dictionary<string, (string En, string Fa)> FacetLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        ["price"] = ("Price", "قیمت"),
        ["inStock"] = ("Availability", "موجودی"),
        ["onSale"] = ("Offers", "پیشنهادها"),
        ["isNew"] = ("Newness", "تازه‌ها"),
        ["minRating"] = ("Rating", "امتیاز"),
        ["color"] = ("Colour", "رنگ"),
        ["size"] = ("Size", "اندازه"),
        ["material"] = ("Material", "جنس"),
        ["room"] = ("Room", "اتاق"),
    };

    private static readonly Dictionary<string, (string En, string Fa)> ValueLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        ["true"] = ("Yes", "بله"),
        ["inStock"] = ("In stock only", "فقط موجود"),
        ["onSale"] = ("On sale", "حراج"),
        ["isNew"] = ("New arrivals", "محصولات جدید"),
        ["under-5m"] = ("Under 5M", "زیر ۵ میلیون"),
        ["5m-10m"] = ("5M – 10M", "۵ تا ۱۰ میلیون"),
        ["10m-15m"] = ("10M – 15M", "۱۰ تا ۱۵ میلیون"),
        ["15m-20m"] = ("15M – 20M", "۱۵ تا ۲۰ میلیون"),
        ["over-20m"] = ("Over 20M", "بالای ۲۰ میلیون"),
        ["4"] = ("4+ stars", "۴ ستاره و بالاتر"),
        ["3"] = ("3+ stars", "۳ ستاره و بالاتر"),
    };

    public static bool IsNewProduct(DateTime createdOnUtc)
        => createdOnUtc >= DateTime.UtcNow.Subtract(NewProductWindow);

    public static CatalogListingDto Apply(
        CatalogListingDto listing,
        GetCatalogListingRequest request,
        bool isFa)
    {
        if (listing.PathNotFound)
            return listing;

        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 12 : Math.Min(request.PageSize, 48);
        var allProducts = listing.Products;

        var filtered = allProducts
            .Where(product => MatchesFilters(product, request))
            .ToList();

        var sorted = SortProducts(filtered, request.Sort);
        var totalCount = sorted.Count;
        var pageProducts = sorted
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var facetGroups = BuildFacetGroups(allProducts, request, isFa, listing.FacetLabels);

        return listing with
        {
            Products = pageProducts,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            FacetGroups = facetGroups,
        };
    }

    private static bool MatchesFilters(CatalogListingProductDto product, GetCatalogListingRequest request)
    {
        if (request.InStock == true && !product.InStock)
            return false;

        if (request.OnSale == true && !product.OnSale)
            return false;

        if (request.IsNew == true && !product.IsNew)
            return false;

        if (request.MinRating is > 0)
        {
            if (product.AverageRating is null || product.AverageRating < request.MinRating)
                return false;
        }

        if (!MatchesPrice(product, request))
            return false;

        foreach (var (facetId, values) in request.AttributeFilters)
        {
            if (values.Count == 0)
                continue;

            if (!product.Facets.TryGetValue(facetId, out var productValues))
                return false;

            if (!values.Any(v => productValues.Contains(v, StringComparer.OrdinalIgnoreCase)))
                return false;
        }

        return true;
    }

    private static bool MatchesPrice(CatalogListingProductDto product, GetCatalogListingRequest request)
    {
        if (request.PriceBuckets.Count > 0)
        {
            return request.PriceBuckets.Any(bucketId =>
            {
                var bucket = PriceBuckets.FirstOrDefault(b =>
                    b.Id.Equals(bucketId, StringComparison.OrdinalIgnoreCase));
                return !string.IsNullOrEmpty(bucket.Id)
                       && product.Price >= bucket.Min
                       && product.Price <= bucket.Max;
            });
        }

        if (request.MinPrice is not null && product.Price < request.MinPrice.Value)
            return false;

        if (request.MaxPrice is not null && product.Price > request.MaxPrice.Value)
            return false;

        return true;
    }

    private static List<CatalogListingProductDto> SortProducts(
        List<CatalogListingProductDto> products,
        string? sort)
    {
        return sort switch
        {
            "price-asc" => products.OrderBy(p => p.Price).ThenBy(p => p.Id).ToList(),
            "price-desc" => products.OrderByDescending(p => p.Price).ThenBy(p => p.Id).ToList(),
            "newest" => products.OrderByDescending(p => p.IsNew).ThenByDescending(p => p.Id).ToList(),
            "best-selling" => products.OrderByDescending(p => p.PurchaseCount).ThenByDescending(p => p.Id).ToList(),
            "rating" => products
                .OrderByDescending(p => p.AverageRating ?? 0)
                .ThenByDescending(p => p.ReviewCount)
                .ThenBy(p => p.Id)
                .ToList(),
            _ => products,
        };
    }

    private static List<CatalogListingFacetGroupDto> BuildFacetGroups(
        List<CatalogListingProductDto> products,
        GetCatalogListingRequest request,
        bool isFa,
        CatalogListingFacetLabelLookupDto? labelLookup)
    {
        if (products.Count == 0)
            return [];

        var groups = new List<CatalogListingFacetGroupDto>();
        var priceBounds = (
            Min: products.Min(p => p.Price),
            Max: products.Max(p => p.Price));

        var priceOptions = PriceBuckets
            .Select(bucket => new CatalogListingFacetOptionDto(
                bucket.Id,
                LabelFor(bucket.Id, isFa),
                CountProducts(products, request, "price", bucket.Id, product =>
                {
                    var match = PriceBuckets.First(b => b.Id == bucket.Id);
                    return product.Price >= match.Min && product.Price <= match.Max;
                })))
            .Where(option => option.Count > 0)
            .ToList();

        groups.Add(new CatalogListingFacetGroupDto(
            "price",
            FacetLabel("price", isFa),
            "range",
            priceOptions,
            new CatalogListingPriceRangeDto(
                priceBounds.Min,
                priceBounds.Max,
                1,
                request.MinPrice,
                request.MaxPrice)));

        AddBooleanFacet(groups, products, request, isFa, "inStock", p => p.InStock, "true");
        AddBooleanFacet(groups, products, request, isFa, "onSale", p => p.OnSale, "true");
        AddBooleanFacet(groups, products, request, isFa, "isNew", p => p.IsNew, "true");

        var ratingOptions = new[] { "4", "3" }
            .Select(value => new CatalogListingFacetOptionDto(
                value,
                LabelFor(value, isFa),
                CountProducts(products, request, "minRating", value, p =>
                    p.AverageRating is not null && p.AverageRating >= double.Parse(value))))
            .Where(option => option.Count > 0)
            .ToList();

        if (ratingOptions.Count > 0)
        {
            groups.Add(new CatalogListingFacetGroupDto(
                "minRating",
                FacetLabel("minRating", isFa),
                "checkbox",
                ratingOptions));
        }

        var attributeFacetIds = products
            .SelectMany(p => p.Facets.Keys)
            .Concat(request.AttributeFilters.Keys)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(id => id, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var facetId in attributeFacetIds)
        {
            var values = products
                .SelectMany(p => p.Facets.TryGetValue(facetId, out var facetValues) ? facetValues : [])
                .Concat(request.AttributeFilters.TryGetValue(facetId, out var activeValues) ? activeValues : [])
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var options = values
                .Select(value => new CatalogListingFacetOptionDto(
                    value,
                    ResolveOptionLabel(facetId, value, isFa, labelLookup),
                    CountProducts(products, request, facetId, value, p =>
                        p.Facets.TryGetValue(facetId, out var facetValues)
                        && facetValues.Contains(value, StringComparer.OrdinalIgnoreCase)),
                    facetId.Equals("color", StringComparison.OrdinalIgnoreCase)
                        ? ResolveColorSwatch(value)
                        : null))
                .Where(option =>
                    option.Count > 0
                    || (request.AttributeFilters.TryGetValue(facetId, out var selectedValues)
                        && selectedValues.Contains(option.Value, StringComparer.OrdinalIgnoreCase)))
                .ToList();

            if (options.Count == 0)
                continue;

            groups.Add(new CatalogListingFacetGroupDto(
                facetId,
                ResolveGroupLabel(facetId, isFa, labelLookup),
                facetId.Equals("color", StringComparison.OrdinalIgnoreCase) ? "color" : "checkbox",
                options));
        }

        return groups;
    }

    private static void AddBooleanFacet(
        List<CatalogListingFacetGroupDto> groups,
        List<CatalogListingProductDto> products,
        GetCatalogListingRequest request,
        bool isFa,
        string facetId,
        Func<CatalogListingProductDto, bool> predicate,
        string value)
    {
        var count = CountProducts(products, request, facetId, value, predicate);
        if (count <= 0)
            return;

        groups.Add(new CatalogListingFacetGroupDto(
            facetId,
            FacetLabel(facetId, isFa),
            "checkbox",
            [new CatalogListingFacetOptionDto(value, LabelFor(facetId, isFa), count)]));
    }

    private static int CountProducts(
        List<CatalogListingProductDto> products,
        GetCatalogListingRequest request,
        string facetId,
        string optionValue,
        Func<CatalogListingProductDto, bool> optionMatch)
    {
        return products.Count(product =>
        {
            var probe = CloneRequestWithoutFacet(request, facetId);
            if (!MatchesFilters(product, probe))
                return false;

            return optionMatch(product);
        });
    }

    private static GetCatalogListingRequest CloneRequestWithoutFacet(
        GetCatalogListingRequest request,
        string facetId)
    {
        var attributeFilters = request.AttributeFilters
            .Where(entry => !entry.Key.Equals(facetId, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(entry => entry.Key, entry => entry.Value, StringComparer.OrdinalIgnoreCase);

        var priceBuckets = facetId.Equals("price", StringComparison.OrdinalIgnoreCase)
            ? []
            : request.PriceBuckets.ToList();

        return request with
        {
            AttributeFilters = attributeFilters,
            PriceBuckets = priceBuckets,
            MinPrice = facetId.Equals("price", StringComparison.OrdinalIgnoreCase) ? null : request.MinPrice,
            MaxPrice = facetId.Equals("price", StringComparison.OrdinalIgnoreCase) ? null : request.MaxPrice,
            InStock = facetId.Equals("inStock", StringComparison.OrdinalIgnoreCase) ? null : request.InStock,
            OnSale = facetId.Equals("onSale", StringComparison.OrdinalIgnoreCase) ? null : request.OnSale,
            IsNew = facetId.Equals("isNew", StringComparison.OrdinalIgnoreCase) ? null : request.IsNew,
            MinRating = facetId.Equals("minRating", StringComparison.OrdinalIgnoreCase) ? null : request.MinRating,
        };
    }

    private static string ResolveGroupLabel(
        string facetId,
        bool isFa,
        CatalogListingFacetLabelLookupDto? labelLookup)
    {
        if (labelLookup?.GroupLabels.TryGetValue(facetId, out var label) == true
            && !string.IsNullOrWhiteSpace(label))
        {
            return label;
        }

        return FacetLabel(facetId, isFa);
    }

    private static string ResolveOptionLabel(
        string facetId,
        string value,
        bool isFa,
        CatalogListingFacetLabelLookupDto? labelLookup)
    {
        if (labelLookup?.OptionLabels.TryGetValue(facetId, out var options) == true
            && options.TryGetValue(value, out var label)
            && !string.IsNullOrWhiteSpace(label))
        {
            return label;
        }

        return LabelFor(value, isFa);
    }

    private static string FacetLabel(string facetId, bool isFa)
    {
        if (FacetLabels.TryGetValue(facetId, out var labels))
            return isFa ? labels.Fa : labels.En;

        return facetId.Replace('-', ' ');
    }

    private static string LabelFor(string key, bool isFa)
    {
        if (ValueLabels.TryGetValue(key, out var labels))
            return isFa ? labels.Fa : labels.En;

        return key.Replace('-', ' ');
    }

    private static string? ResolveColorSwatch(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "green" => "#2d6a4f",
            "grey" or "gray" => "#6b7280",
            "walnut" => "#5c4033",
            "oak" => "#c4a35a",
            "white" => "#f5f5f4",
            "beige" => "#d4c4a8",
            "cream" => "#fffdd0",
            "natural" => "#e8dcc8",
            "brass" => "#b5a642",
            "clear" => "#e5e7eb",
            "neutral" => "#d6d3d1",
            "taupe" => "#b8a99a",
            "charcoal" => "#36454f",
            "teak" => "#ab8953",
            _ => null,
        };
    }
}
