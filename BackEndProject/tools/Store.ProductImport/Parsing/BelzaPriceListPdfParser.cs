using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Store.ProductImport.Models;
using UglyToad.PdfPig;

namespace Store.ProductImport.Parsing;

/// <summary>
/// Parser for Belza price-list PDFs (Store Files folder): row, name, code, carton qty, sale price, consumer price.
/// </summary>
public static class BelzaPriceListPdfParser
{
    /// <summary>
    /// Belza product codes: T-89201, SO 002411, CO-201, ST-88332, SF-7-2413-P, GES-2227-04, SDI-50020, …
    /// </summary>
    private const string ProductCodeGroup = @"[A-Z]{1,4}(?:[-\s][A-Z0-9]+)+";

    private static readonly Regex DataRowPattern = new(
        $@"^(?<row>[\d۰-۹]{{1,3}})[\s\t]+(?<name>.+?)[\s\t]+(?<code>{ProductCodeGroup})[\s\t]+(?<tail>.+)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex GlobalRowPattern = new(
        $@"(?<row>[\d۰-۹]{{1,3}})[\s\t]+(?<name>[^\n\r]+?)[\s\t]+(?<code>{ProductCodeGroup})[\s\t]+(?<tail>[^\n\r]+)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex ProductCodeTokenPattern = new(
        $@"^{ProductCodeGroup}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly HashSet<string> CartonUnits = new(StringComparer.Ordinal)
    {
        "عدد",
        "جفت",
        "دست",
    };

    public static bool IsBelzaFormat(string text)
        => text.Contains("بلزا", StringComparison.Ordinal)
           || text.Contains("محصولات تولیدی", StringComparison.Ordinal)
           || text.Contains("محصوالت تولیدی", StringComparison.Ordinal)
           || Regex.IsMatch(text, ProductCodeGroup, RegexOptions.IgnoreCase);

    public static List<ParsedInventoryProduct> Parse(string text)
    {
        text = NormalizePersianDigits(text.Replace("\r\n", "\n"));
        var byCode = new Dictionary<string, ParsedInventoryProduct>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in text.Split('\n'))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || IsSkippedLine(line))
                continue;

            if (TryParseLine(line, out var product))
                TryAdd(byCode, product);
        }

        foreach (Match match in GlobalRowPattern.Matches(text))
        {
            if (!TryParseRowMatch(match, out var product))
                continue;

            TryAdd(byCode, product);
        }

        return byCode.Values
            .OrderBy(p => p.Row)
            .ThenBy(p => p.ProductCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static List<ParsedInventoryProduct> ParsePdf(string pdfPath)
    {
        var byCode = new Dictionary<string, ParsedInventoryProduct>(StringComparer.OrdinalIgnoreCase);

        foreach (var text in new[]
                 {
                     InventoryPdfParser.ExtractText(pdfPath),
                     ExtractLayoutText(pdfPath),
                     ExtractSimplePageText(pdfPath),
                 })
        {
            foreach (var product in Parse(text))
                TryAdd(byCode, product);
        }

        var products = byCode.Values
            .OrderBy(p => p.Row)
            .ThenBy(p => p.ProductCode, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var product in products)
            ProductTextHelper.EnrichBelza(product);

        return products;
    }

    private static void TryAdd(Dictionary<string, ParsedInventoryProduct> byCode, ParsedInventoryProduct product)
    {
        if (string.IsNullOrWhiteSpace(product.ProductCode))
            return;

        if (!byCode.TryGetValue(product.ProductCode, out var existing))
        {
            byCode[product.ProductCode] = product;
            return;
        }

        // Prefer the row with a longer / cleaner title.
        if (product.TitleFa.Length > existing.TitleFa.Length)
            byCode[product.ProductCode] = product;
    }

    /// <summary>Rebuild lines from word bounding boxes (better for RTL tables).</summary>
    private static string ExtractLayoutText(string pdfPath)
    {
        var builder = new StringBuilder();
        using var document = PdfDocument.Open(pdfPath);

        foreach (var page in document.GetPages())
        {
            const double rowTolerance = 4d;
            var rows = page.GetWords()
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .ThenBy(w => w.BoundingBox.Left)
                .GroupBy(w => Math.Round(w.BoundingBox.Bottom / rowTolerance) * rowTolerance)
                .OrderByDescending(g => g.Key);

            foreach (var row in rows)
            {
                var line = string.Join("\t", row.OrderBy(w => w.BoundingBox.Left).Select(w => w.Text));
                builder.AppendLine(line);
            }
        }

        return builder.ToString();
    }

    private static string ExtractSimplePageText(string pdfPath)
    {
        var builder = new StringBuilder();
        using var document = PdfDocument.Open(pdfPath);
        foreach (var page in document.GetPages())
            builder.AppendLine(page.Text);

        return builder.ToString();
    }

    private static bool TryParseLine(string line, out ParsedInventoryProduct product)
    {
        product = null!;

        var match = DataRowPattern.Match(line);
        if (!match.Success)
            return TryParseLineByCodeAnchor(line, out product);

        return TryParseRowMatch(match, out product);
    }

    /// <summary>When columns are merged, locate the product-code token and split name/tail around it.</summary>
    private static bool TryParseLineByCodeAnchor(string line, out ParsedInventoryProduct product)
    {
        product = null!;

        var rowMatch = Regex.Match(line, @"^(?<row>[\d۰-۹]{1,3})[\s\t]+(?<rest>.+)$");
        if (!rowMatch.Success)
            return false;

        if (!TryParseRowNumber(rowMatch.Groups["row"].Value, out var row))
            return false;

        var rest = rowMatch.Groups["rest"].Value;
        var tokens = Regex.Split(rest, @"[\s\t]+")
            .Where(t => t.Length > 0)
            .ToArray();

        var codeIndex = -1;
        for (var i = 0; i < tokens.Length; i++)
        {
            if (ProductCodeTokenPattern.IsMatch(tokens[i]))
            {
                codeIndex = i;
                break;
            }
        }

        if (codeIndex <= 0)
            return false;

        var name = string.Join(" ", tokens.Take(codeIndex)).Trim();
        var code = tokens[codeIndex];
        var tail = string.Join(" ", tokens.Skip(codeIndex + 1)).Trim();

        return TryBuildProduct(row, name, code, tail, out product);
    }

    private static bool TryParseRowMatch(Match match, out ParsedInventoryProduct product)
    {
        product = null!;

        if (!TryParseRowNumber(match.Groups["row"].Value, out var row))
            return false;

        return TryBuildProduct(
            row,
            match.Groups["name"].Value.Trim(),
            match.Groups["code"].Value.Trim(),
            match.Groups["tail"].Value.Trim(),
            out product);
    }

    private static bool TryBuildProduct(
        int row,
        string name,
        string rawCode,
        string tail,
        out ParsedInventoryProduct product)
    {
        product = null!;

        name = Regex.Replace(name, @"\s+", " ").Trim();
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var productCode = NormalizeProductCode(rawCode);
        if (productCode.Length == 0 || productCode.Length > 10)
            return false;

        ParseTailString(tail, out var cartonQty, out var cartonUnit, out var purchasePrice, out var consumerPrice, out var discontinued);

        product = new ParsedInventoryProduct
        {
            CatalogKind = ImportCatalogKind.Belza,
            Row = row,
            TitleFa = name,
            ExternalCode = productCode,
            ProductCode = productCode,
            PackQuantity = cartonQty,
            PackUnitFa = cartonUnit,
            PurchasePriceToman = purchasePrice,
            PriceToman = consumerPrice,
            IsDiscontinued = discontinued,
        };

        return true;
    }

    private static bool TryParseRowNumber(string raw, out int row)
    {
        raw = NormalizePersianDigits(raw.Trim());
        return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out row);
    }

    private static bool IsSkippedLine(string line)
    {
        if (line.StartsWith("لیست", StringComparison.Ordinal)
            || line.StartsWith("خانواده", StringComparison.Ordinal)
            || line.StartsWith("ردیف", StringComparison.Ordinal)
            || line.StartsWith("نام کا", StringComparison.Ordinal)
            || line.StartsWith("کد کا", StringComparison.Ordinal)
            || line.StartsWith("تعداد در", StringComparison.Ordinal)
            || line.StartsWith("قیمت", StringComparison.Ordinal)
            || line.StartsWith("تسویه", StringComparison.Ordinal)
            || line.StartsWith("-- ", StringComparison.Ordinal)
            || line.StartsWith("(", StringComparison.Ordinal)
            || line.Contains("محصولات تولیدی", StringComparison.Ordinal)
            || line.Contains("محصوالت تولیدی", StringComparison.Ordinal))
        {
            return true;
        }

        return Regex.IsMatch(line, @"^[\d۰-۹]{1,3}\.\s*$");
    }

    private static string NormalizeProductCode(string raw)
    {
        var code = raw.Trim().ToUpperInvariant();
        code = Regex.Replace(code, @"\s+", " ");

        var compact = code.Replace(" ", "");
        if (compact.Length <= 10)
            return compact;

        compact = compact.Replace("-", "");
        if (compact.Length <= 10)
            return compact;

        return compact[..10];
    }

    private static string NormalizePersianDigits(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var builder = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            builder.Append(ch switch
            {
                '۰' => '0',
                '۱' => '1',
                '۲' => '2',
                '۳' => '3',
                '۴' => '4',
                '۵' => '5',
                '۶' => '6',
                '۷' => '7',
                '۸' => '8',
                '۹' => '9',
                _ => ch,
            });
        }

        return builder.ToString();
    }

    private static void ParseTailString(
        string tail,
        out string cartonQty,
        out string cartonUnit,
        out decimal purchasePrice,
        out decimal consumerPrice,
        out bool discontinued)
    {
        cartonQty = string.Empty;
        cartonUnit = string.Empty;
        purchasePrice = 0;
        consumerPrice = 0;
        discontinued = false;

        if (string.IsNullOrWhiteSpace(tail))
            return;

        tail = Regex.Replace(tail.Trim(), @"[\s\t]+", " ");
        tail = Regex.Replace(tail, @"توقف\s+تولید", "توقف_تولید", RegexOptions.IgnoreCase);

        var tokens = tail.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        if (tokens.Count == 0)
            return;

        var consumerRaw = PopToken(tokens);
        var purchaseRaw = PopToken(tokens);

        consumerPrice = ParsePriceToken(consumerRaw, out var consumerStopped);
        purchasePrice = ParsePriceToken(purchaseRaw, out var purchaseStopped);
        discontinued = consumerStopped || purchaseStopped;

        if (tokens.Count == 0)
            return;

        if (tokens.Count >= 2 && CartonUnits.Contains(tokens[^1]))
        {
            cartonUnit = tokens[^1];
            cartonQty = tokens[^2];
            return;
        }

        if (tokens.Count == 1)
        {
            if (CartonUnits.Contains(tokens[0]))
                cartonUnit = tokens[0];
            else if (!IsMissingCartonMarker(tokens[0]))
                cartonQty = tokens[0];
            return;
        }

        cartonQty = tokens[0];
        cartonUnit = string.Join(" ", tokens.Skip(1)).Trim();
    }

    private static string PopToken(List<string> tokens)
    {
        if (tokens.Count == 0)
            return string.Empty;

        var last = tokens[^1];
        tokens.RemoveAt(tokens.Count - 1);

        if (last.Equals("تولید", StringComparison.Ordinal)
            && tokens.Count > 0
            && tokens[^1].Contains("توقف", StringComparison.Ordinal))
        {
            last = $"{tokens[^1]} {last}".Trim();
            tokens.RemoveAt(tokens.Count - 1);
        }

        return last;
    }

    private static decimal ParsePriceToken(string raw, out bool discontinued)
    {
        discontinued = false;
        if (string.IsNullOrWhiteSpace(raw) || IsMissingCartonMarker(raw))
            return 0;

        if (raw.Contains("توقف", StringComparison.Ordinal) || raw.Contains("توقف_تولید", StringComparison.Ordinal))
        {
            discontinued = true;
            return 0;
        }

        var digits = raw.Replace(",", string.Empty, StringComparison.Ordinal).Trim();
        return decimal.TryParse(digits, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            ? value
            : 0;
    }

    private static bool IsMissingCartonMarker(string value)
        => value is "_" or "-" or "—";
}
