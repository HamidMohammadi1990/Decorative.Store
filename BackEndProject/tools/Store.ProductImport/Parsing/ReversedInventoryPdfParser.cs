using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Store.ProductImport.Models;

namespace Store.ProductImport.Parsing;

/// <summary>
/// Parser for inventory PDFs where PdfPig extracts Persian RTL text in reversed character order
/// and concatenates row number + barcode (e.g. 18693357271050 = row 1 + 8693357271050).
/// </summary>
public static class ReversedInventoryPdfParser
{
    private const string BarcodePrefix = "869335";

    private static readonly Regex RowBarcodeRegex = new(
        $@"(\d{{1,4}})({BarcodePrefix}\d{{7}})",
        RegexOptions.Compiled);

    private static readonly Regex StandardTailRegex = new(
        @"\((\d{4,7})\)\s*\1(\d+)(تفج|ددع|تسد)([\d,]+)",
        RegexOptions.Compiled);

    private static readonly Regex BrokenTailRegex = new(
        @"\((\d{4,7})\)\s*\1(\d+)\s+(\S+).*?(?:یددع)?([\d,]+)",
        RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex NoParenTailRegex = new(
        @"(?:sb|gb|sl)\s*(\d{4,7})(\d+)(تفج|ددع|تسد)([\d,]+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static bool IsReversedFormat(string text)
        => text.Contains("هچغاباشاپ", StringComparison.Ordinal)
           || text.Contains("یددع", StringComparison.Ordinal)
           || text.Contains("فیدرالاک", StringComparison.Ordinal);

    public static List<ParsedInventoryProduct> Parse(string text)
    {
        text = text.Replace("\r\n", " ").Replace('\n', ' ');
        text = Regex.Replace(text, @"\s+", " ");

        var products = new List<ParsedInventoryProduct>();
        var consumed = new HashSet<int>();

        foreach (Match tail in StandardTailRegex.Matches(text))
        {
            if (consumed.Contains(tail.Index))
                continue;

            if (TryBuildFromTail(text, tail, tail.Groups[1].Value, tail.Groups[2].Value,
                    UnreversePackUnit(tail.Groups[3].Value), tail.Groups[4].Value, out var product))
            {
                products.Add(product);
                consumed.Add(tail.Index);
            }
        }

        foreach (Match tail in BrokenTailRegex.Matches(text))
        {
            if (consumed.Contains(tail.Index))
                continue;

            var unit = tail.Groups[3].Value switch
            {
                "راهچ" => "چهار عددی",
                _ => UnreversePackUnit(tail.Groups[3].Value),
            };

            if (TryBuildFromTail(text, tail, tail.Groups[1].Value, tail.Groups[2].Value,
                    unit, tail.Groups[4].Value, out var product))
            {
                products.Add(product);
                consumed.Add(tail.Index);
            }
        }

        foreach (Match tail in NoParenTailRegex.Matches(text))
        {
            if (consumed.Contains(tail.Index))
                continue;

            if (TryBuildFromTail(text, tail, tail.Groups[1].Value, tail.Groups[2].Value,
                    UnreversePackUnit(tail.Groups[3].Value), tail.Groups[4].Value, out var product))
            {
                products.Add(product);
                consumed.Add(tail.Index);
            }
        }

        return products
            .GroupBy(p => $"{p.Row}:{p.Barcode}:{p.ExternalCode}", StringComparer.Ordinal)
            .Select(g => g.First())
            .OrderBy(p => p.Row)
            .ToList();
    }

    private static bool TryBuildFromTail(
        string text,
        Match tail,
        string externalCode,
        string packQty,
        string packUnit,
        string priceRaw,
        out ParsedInventoryProduct product)
    {
        product = null!;

        var priceToman = ParseRialToToman(priceRaw);
        if (priceToman <= 0)
            return false;

        var searchEnd = tail.Index;
        var searchStart = Math.Max(0, searchEnd - 600);
        var window = text[searchStart..searchEnd];

        var rowMatches = RowBarcodeRegex.Matches(window);
        if (rowMatches.Count == 0)
            return false;

        var rowMatch = rowMatches[^1];
        var row = int.Parse(rowMatch.Groups[1].Value, CultureInfo.InvariantCulture);
        var barcode = rowMatch.Groups[2].Value;

        var contentStart = rowMatch.Index + rowMatch.Length;
        var content = window[contentStart..].Trim();

        var parenRel = content.IndexOf('(');
        var titleAndMeasure = parenRel >= 0 ? content[..parenRel].Trim() : content;

        SplitTitleAndMeasurement(titleAndMeasure, out var titleFa, out var measurementFa);

        if (string.IsNullOrWhiteSpace(titleFa))
            return false;

        product = new ParsedInventoryProduct
        {
            Row = row,
            Barcode = barcode,
            TitleFa = titleFa,
            ExternalCode = externalCode,
            PackQuantity = packQty,
            PackUnitFa = packUnit,
            MeasurementFa = measurementFa,
            PriceToman = priceToman,
        };

        return true;
    }

    private static void SplitTitleAndMeasurement(string titleAndMeasure, out string titleFa, out string measurementFa)
    {
        titleFa = string.Empty;
        measurementFa = string.Empty;

        if (string.IsNullOrWhiteSpace(titleAndMeasure))
            return;

        var yedaIdx = titleAndMeasure.IndexOf("یددع", StringComparison.Ordinal);
        string titleRaw;
        string measureRaw;

        if (yedaIdx >= 0)
        {
            titleRaw = titleAndMeasure[..yedaIdx].Trim();
            measureRaw = titleAndMeasure[yedaIdx..].Trim();
        }
        else
        {
            titleRaw = titleAndMeasure;
            measureRaw = string.Empty;
        }

        titleFa = CleanupReversedTitle(ReverseRtlText(titleRaw));
        measurementFa = string.IsNullOrWhiteSpace(measureRaw)
            ? string.Empty
            : CleanupMeasurement(ReverseRtlText(measureRaw));
    }

    private static string ReverseRtlText(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var chars = input.Where(c => !char.IsControl(c)).ToArray();
        Array.Reverse(chars);
        return new string(chars).Trim();
    }

    private static string CleanupReversedTitle(string title)
    {
        title = Regex.Replace(title, @"\s+", " ").Trim();
        title = title.Replace("پاشا باغچه", "پاشاباغچه", StringComparison.Ordinal);

        if (title.Length > 1 && char.IsDigit(title[0]) && title[1] == ' ')
            title = title[2..].Trim() + " " + title[0];

        return title;
    }

    private static string CleanupMeasurement(string measurement)
    {
        measurement = Regex.Replace(measurement, @"\s+", " ").Trim();
        measurement = Regex.Replace(measurement, @"\b(sb|gb|sl)\b", string.Empty, RegexOptions.IgnoreCase).Trim();
        return measurement;
    }

    private static string UnreversePackUnit(string reversed) => reversed switch
    {
        "تفج" => "جفت",
        "ددع" => "عدد",
        "تسد" => "دست",
        "راهچ" => "چهار عددی",
        _ => ReverseRtlText(reversed),
    };

    private static decimal ParseRialToToman(string raw)
    {
        var digits = raw.Replace(",", string.Empty, StringComparison.Ordinal).Trim();
        if (!decimal.TryParse(digits, NumberStyles.Number, CultureInfo.InvariantCulture, out var rial))
            return 0;
        return Math.Round(rial / 10m, 0, MidpointRounding.AwayFromZero);
    }
}
