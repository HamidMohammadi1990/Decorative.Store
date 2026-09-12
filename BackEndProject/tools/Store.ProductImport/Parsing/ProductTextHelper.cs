using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Store.ProductImport.Models;

namespace Store.ProductImport.Parsing;

public static class ProductTextHelper
{
    private static readonly (string Fa, string En)[] TermReplacements =
    [
        ("پاشاباغچه", "Pasabahce"),
        ("پاشا باغچه", "Pasabahce"),
        ("فنجان و نعلبکی", "Cup and Saucer Set"),
        ("فنجان", "Cup"),
        ("نعلبکی", "Saucer"),
        ("لیوان دسته دار", "Handled Glass"),
        ("لیوان", "Glass"),
        ("ماگ", "Mug"),
        ("ست فنجان", "Cup Set"),
        ("ظرف نگهدارنده", "Storage Container"),
        ("ظرف غذا", "Food Container"),
        ("کاسه", "Bowl"),
        ("بشقاب", "Plate"),
        ("درب", "Lid"),
        ("سبز", "Green"),
        ("آبی", "Blue"),
        ("قرمز", "Red"),
        ("سفید", "White"),
        ("مشکی", "Black"),
        ("طوسی", "Gray"),
        ("رنگی", "Multicolor"),
        ("متالیک", "Metallic"),
        ("آنتی باکتریال", "Antibacterial"),
        ("عددی", "piece(s)"),
        ("جفت", "pair(s)"),
        ("دست", "set(s)"),
        ("عدد", "unit(s)"),
        ("سی سی", "cc"),
        ("ست", "Set"),
    ];

    public static void Enrich(ParsedInventoryProduct product)
    {
        product.TitleFa = NormalizeFaTitle(product.TitleFa);
        product.TitleEn = TranslateTitle(product.TitleFa);
        product.SlugFa = BuildSlug(product.TitleFa, product.ExternalCode, latin: false);
        product.SlugEn = BuildSlug(product.TitleEn, product.ExternalCode, latin: true);
        product.SubCategorySlug = ResolveSubCategorySlug(product.TitleFa);
        product.DescriptionFa = BuildDescriptionFa(product);
        product.DescriptionEn = BuildDescriptionEn(product);
    }

    public static void EnrichBelza(ParsedInventoryProduct product)
    {
        product.TitleFa = NormalizeFaTitle(product.TitleFa);
        product.TitleEn = TranslateBelzaTitle(product.TitleFa);
        product.SlugFa = BuildSlug(product.TitleFa, product.ProductCode, latin: false);
        product.SlugEn = BuildSlug(product.TitleEn, product.ProductCode, latin: true);
        product.SubCategorySlug = ResolveBelzaSubCategorySlug(product.TitleFa);
        product.DescriptionFa = BuildBelzaDescriptionFa(product);
        product.DescriptionEn = BuildBelzaDescriptionEn(product);
    }

    private static string NormalizeFaTitle(string title)
    {
        var normalized = Regex.Replace(title, @"\s+", " ").Trim();
        normalized = normalized.Replace("پاشا باغچه", "پاشاباغچه", StringComparison.Ordinal);
        return normalized;
    }

    public static string TranslateTitle(string titleFa)
    {
        var result = titleFa;
        foreach (var (fa, en) in TermReplacements.OrderByDescending(x => x.Fa.Length))
            result = result.Replace(fa, en, StringComparison.Ordinal);

        result = Regex.Replace(result, @"\s+", " ").Trim();
        if (!result.StartsWith("Pasabahce", StringComparison.OrdinalIgnoreCase))
            result = $"Pasabahce {result}";

        return result;
    }

    public static string BuildSlug(string title, string externalCode, bool latin)
    {
        var working = title;
        if (!latin)
            working = working.Replace("پاشاباغچه", "پاشا-باغچه", StringComparison.Ordinal);

        var builder = new StringBuilder();
        foreach (var ch in working)
        {
            if (char.IsLetterOrDigit(ch) || (ch >= '\u0600' && ch <= '\u06FF'))
                builder.Append(ch);
            else if (char.IsWhiteSpace(ch) || ch is '-' or '_' or '.')
                builder.Append('-');
        }

        var slug = builder.ToString().ToLowerInvariant();
        slug = Regex.Replace(slug, @"-+", "-").Trim('-');
        slug = $"{slug}-{externalCode}".Trim('-');
        if (slug.Length > 150)
            slug = slug[..150].Trim('-');

        return slug;
    }

    private static string ResolveSubCategorySlug(string titleFa)
    {
        if (titleFa.Contains("ماگ", StringComparison.Ordinal))
            return "kitchen-dining/mugs";
        if (titleFa.Contains("فنجان", StringComparison.Ordinal) || titleFa.Contains("نعلبکی", StringComparison.Ordinal))
            return "kitchen-dining/sets";
        if (titleFa.Contains("کاسه", StringComparison.Ordinal) || titleFa.Contains("بشقاب", StringComparison.Ordinal))
            return "kitchen-dining/plates";
        if (titleFa.Contains("ظرف", StringComparison.Ordinal))
            return "kitchen-dining/serveware";
        if (titleFa.Contains("لیوان", StringComparison.Ordinal))
            return "kitchen-dining/glassware";
        return "kitchen-dining/glassware";
    }

    public static string ResolveBelzaSubCategorySlug(string titleFa)
    {
        if (titleFa.Contains("کاسه", StringComparison.Ordinal))
            return "kitchen-dining/bowls";
        if (titleFa.Contains("بشقاب", StringComparison.Ordinal)
            || titleFa.Contains("پیش دستی", StringComparison.Ordinal)
            || titleFa.Contains("پیاله", StringComparison.Ordinal)
            || titleFa.Contains("سینی", StringComparison.Ordinal))
            return "kitchen-dining/plates";
        if (titleFa.Contains("اردو", StringComparison.Ordinal))
            return "kitchen-dining/sets";
        if (titleFa.Contains("گلدان", StringComparison.Ordinal))
            return "garden/planters-indoor";
        if (titleFa.Contains("میوه", StringComparison.Ordinal)
            || titleFa.Contains("شیرینی", StringComparison.Ordinal)
            || titleFa.Contains("شکلات", StringComparison.Ordinal)
            || titleFa.Contains("قندان", StringComparison.Ordinal)
            || titleFa.Contains("استند", StringComparison.Ordinal)
            || titleFa.Contains("شمعدان", StringComparison.Ordinal)
            || titleFa.Contains("سطل", StringComparison.Ordinal)
            || titleFa.Contains("جا دستمال", StringComparison.Ordinal)
            || titleFa.Contains("جاکاردی", StringComparison.Ordinal)
            || titleFa.Contains("انگاره", StringComparison.Ordinal))
            return "kitchen-dining/serveware";

        return "kitchen-dining/serveware";
    }

    public static string TranslateBelzaTitle(string titleFa)
    {
        var result = titleFa;
        foreach (var (fa, en) in TermReplacements.OrderByDescending(x => x.Fa.Length))
            result = result.Replace(fa, en, StringComparison.Ordinal);

        result = Regex.Replace(result, @"\s+", " ").Trim();
        if (!result.StartsWith("Belza", StringComparison.OrdinalIgnoreCase))
            result = $"Belza {result}";

        return result;
    }

    private static string BuildDescriptionFa(ParsedInventoryProduct product)
    {
        var measurement = string.IsNullOrWhiteSpace(product.MeasurementFa)
            ? string.Empty
            : $" ظرفیت/اندازه: {product.MeasurementFa}.";

        return
            $"{product.TitleFa} از برند پاشاباغچه با کد خارجی {product.ExternalCode}.{measurement} " +
            $"هر بسته شامل {product.PackQuantity} {product.PackUnitFa} است. " +
            "محصول اصل با بسته‌بندی استاندارد و مناسب سرو و پذیرایی.";
    }

    private static string BuildDescriptionEn(ParsedInventoryProduct product)
    {
        var measurement = string.IsNullOrWhiteSpace(product.MeasurementFa)
            ? string.Empty
            : $" Capacity/size: {TranslateTitle(product.MeasurementFa)}.";

        var packUnitEn = TranslateTitle(product.PackUnitFa);

        return
            $"{product.TitleEn} by Pasabahce (external ref. {product.ExternalCode}).{measurement} " +
            $"Each wholesale pack contains {product.PackQuantity} {packUnitEn}. " +
            "Authentic product suitable for dining and hospitality use.";
    }

    public static string TranslatePackValue(string valueFa)
        => TranslateTitle(valueFa);

    private static string BuildBelzaDescriptionFa(ParsedInventoryProduct product)
    {
        var carton = string.IsNullOrWhiteSpace(product.PackQuantity)
            ? string.Empty
            : $" هر کارتن شامل {product.PackQuantity} {product.PackUnitFa} است.";

        var status = product.IsDiscontinued ? " (توقف تولید)" : string.Empty;

        return
            $"{product.TitleFa} با کد {product.ProductCode}.{carton}{status} " +
            "محصول تولیدی بلزا مناسب سرو و پذیرایی.";
    }

    private static string BuildBelzaDescriptionEn(ParsedInventoryProduct product)
    {
        var carton = string.IsNullOrWhiteSpace(product.PackQuantity)
            ? string.Empty
            : $" Each carton contains {product.PackQuantity} {TranslatePackValue(product.PackUnitFa)}.";

        var status = product.IsDiscontinued ? " (discontinued)" : string.Empty;

        return
            $"{product.TitleEn} (code {product.ProductCode}).{carton}{status} " +
            "Belza product suitable for dining and hospitality use.";
    }
}
