using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Store.ProductImport.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Store.ProductImport.Parsing;

public static class InventoryPdfParser
{
    private static readonly Regex ExternalCodeRegex = new(@"\(\s*(\d{4,7})\s*\)", RegexOptions.Compiled);
    private static readonly Regex PriceTokenRegex = new(@"\b([\d]{1,3}(?:,[\d]{3})+)\b", RegexOptions.Compiled);
    private static readonly Regex ProductStartRegex = new(@"^(\d{1,4})[\t\s]+(\d{13})\s*$", RegexOptions.Compiled);
    private static readonly Regex BarcodeOnlyRegex = new(@"^\d{13}$", RegexOptions.Compiled);
    private static readonly Regex RowOnlyRegex = new(@"^\d{1,4}$", RegexOptions.Compiled);
    private static readonly Regex TailLineRegex = new(
        @"^(\d{4,7})[\t\s]+(\d+)[\t\s]+(.+?)[\t\s]+([\d,]+)\s*$",
        RegexOptions.Compiled);

    public static IReadOnlyList<ParsedInventoryProduct> ParsePdf(string pdfPath)
    {
        var text = ExtractText(pdfPath);
        var images = ExtractImages(pdfPath);
        var products = ParseText(text);

        for (var i = 0; i < products.Count && i < images.Count; i++)
        {
            products[i].ImageBytes = images[i].Bytes;
            products[i].ImageExtension = images[i].Extension;
        }

        return products;
    }

    public static string ExtractText(string pdfPath)
    {
        var builder = new StringBuilder();
        using var document = PdfDocument.Open(pdfPath);
        foreach (var page in document.GetPages())
        {
            var pageText = ContentOrderTextExtractor.GetText(page);
            if (string.IsNullOrWhiteSpace(pageText))
                pageText = page.Text;

            builder.AppendLine(pageText);
        }

        return builder.ToString();
    }

    private static List<(byte[] Bytes, string Extension)> ExtractImages(string pdfPath)
    {
        var images = new List<(byte[] Bytes, string Extension)>();
        using var document = PdfDocument.Open(pdfPath);
        foreach (var page in document.GetPages())
        {
            foreach (var image in page.GetImages())
            {
                var raw = ExtractImageBytes(image);
                if (raw is null || raw.Length < 512)
                    continue;

                var extension = GuessExtension(raw);
                images.Add((raw, extension));
            }
        }

        return images;
    }

    private static byte[]? ExtractImageBytes(IPdfImage image)
    {
        if (image.TryGetPng(out var pngBytes) && pngBytes.Length > 0)
            return pngBytes;

        if (image.TryGetBytesAsMemory(out Memory<byte> decoded) && decoded.Length > 0)
            return decoded.ToArray();

        var rawMemory = image.RawMemory;
        if (rawMemory.Length > 0)
            return rawMemory.ToArray();

        return null;
    }

    private static string GuessExtension(byte[] bytes)
    {
        if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8)
            return ".jpg";
        if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50)
            return ".png";
        return ".jpg";
    }

    internal static string NormalizeExtractedText(string text)
    {
        text = text.Replace("\r\n", "\n");

        // PdfPig often merges table cells — force row/barcode boundaries.
        text = Regex.Replace(text, @"(\d{1,4})[\t ]+(\d{13})", "\n$1\t$2");
        text = Regex.Replace(text, @"\)\s*(\d{4,7})[\t ]+(\d+)[\t ]+", ")\n$1\t$2\t");
        text = Regex.Replace(text, @"\n{3,}", "\n\n");

        return text;
    }

    internal static List<ParsedInventoryProduct> ParseText(string text)
    {
        List<ParsedInventoryProduct> products;

        if (ReversedInventoryPdfParser.IsReversedFormat(text))
        {
            products = ReversedInventoryPdfParser.Parse(text);
        }
        else
        {
            text = NormalizeExtractedText(text);
            products = ParseTextByLines(text);
            if (products.Count == 0)
                products = ParseTextByBarcodeScan(text);
        }

        AssignProductCodes(products);
        foreach (var product in products)
            ProductTextHelper.Enrich(product);

        return products;
    }

    private static List<ParsedInventoryProduct> ParseTextByLines(string text)
    {
        var lines = text
            .Split('\n')
            .Select(static line => line.Trim())
            .Where(static line => line.Length > 0)
            .ToList();

        var products = new List<ParsedInventoryProduct>();
        for (var i = 0; i < lines.Count; i++)
        {
            if (!TryParseProductStart(lines, i, out var row, out var barcode, out var chunkStart))
                continue;

            var chunk = new List<string>();
            for (var j = chunkStart; j < lines.Count; j++)
            {
                if (IsProductStartLine(lines, j) || lines[j].StartsWith("-- ", StringComparison.Ordinal))
                    break;

                if (!IsHeaderLine(lines[j]))
                    chunk.Add(lines[j]);
            }

            if (TryParseChunk(row, barcode, chunk, out var product))
                products.Add(product);
        }

        return products;
    }

    private static List<ParsedInventoryProduct> ParseTextByBarcodeScan(string text)
    {
        var products = new List<ParsedInventoryProduct>();
        var matches = Regex.Matches(text, @"(\d{1,4})[\t\s]+(\d{13})");
        for (var m = 0; m < matches.Count; m++)
        {
            var start = matches[m].Index;
            var end = m + 1 < matches.Count ? matches[m + 1].Index : text.Length;
            var block = text[start..end];

            var lines = block
                .Split('\n')
                .Select(static line => line.Trim())
                .Where(static line => line.Length > 0 && !IsHeaderLine(line))
                .ToList();

            if (lines.Count == 0)
                continue;

            if (!TryParseProductStart(lines, 0, out var row, out var barcode, out var chunkStart))
                continue;

            var chunk = lines.Skip(chunkStart).ToList();
            if (TryParseChunk(row, barcode, chunk, out var product))
                products.Add(product);
        }

        return products;
    }

    private static bool IsProductStartLine(IReadOnlyList<string> lines, int index)
        => TryParseProductStart(lines, index, out _, out _, out _);

    private static bool TryParseProductStart(
        IReadOnlyList<string> lines,
        int index,
        out int row,
        out string barcode,
        out int chunkStart)
    {
        row = 0;
        barcode = string.Empty;
        chunkStart = index + 1;

        var line = lines[index];
        var inline = ProductStartRegex.Match(line);
        if (inline.Success)
        {
            row = int.Parse(inline.Groups[1].Value, CultureInfo.InvariantCulture);
            barcode = inline.Groups[2].Value;
            return true;
        }

        if (BarcodeOnlyRegex.IsMatch(line) && index > 0 && RowOnlyRegex.IsMatch(lines[index - 1]))
        {
            row = int.Parse(lines[index - 1], CultureInfo.InvariantCulture);
            barcode = line;
            chunkStart = index + 1;
            return true;
        }

        var embedded = Regex.Match(line, @"^(\d{1,4})[\t\s]+(\d{13})\b");
        if (embedded.Success)
        {
            row = int.Parse(embedded.Groups[1].Value, CultureInfo.InvariantCulture);
            barcode = embedded.Groups[2].Value;
            chunkStart = index + 1;
            return true;
        }

        return false;
    }

    private static bool IsHeaderLine(string line)
        => line.StartsWith("ردیف", StringComparison.Ordinal)
           || line.StartsWith("تعداد در", StringComparison.Ordinal)
           || line.StartsWith("واحد", StringComparison.Ordinal)
           || line.StartsWith("اندازه", StringComparison.Ordinal)
           || line.StartsWith("قیمت فروش", StringComparison.Ordinal)
           || line.StartsWith("عکس کالا", StringComparison.Ordinal)
           || line.StartsWith("بارکد", StringComparison.Ordinal)
           || line is "گیری" or "بسته" or "جاری" or "نام" or "کد خارجی";

    private static bool TryParseChunk(
        int row,
        string barcode,
        IReadOnlyList<string> chunk,
        out ParsedInventoryProduct product)
    {
        product = null!;

        if (chunk.Count == 0)
            return false;

        var externalCode = ExtractExternalCode(chunk);
        if (string.IsNullOrWhiteSpace(externalCode))
            return false;

        if (!TryParsePriceTail(chunk, externalCode, out var packQty, out var packUnit, out var priceToman))
            return false;

        var title = BuildTitle(chunk, externalCode);
        if (string.IsNullOrWhiteSpace(title))
            return false;

        var measurement = ExtractMeasurement(chunk, externalCode);

        product = new ParsedInventoryProduct
        {
            Row = row,
            Barcode = barcode,
            TitleFa = title,
            ExternalCode = externalCode,
            PackQuantity = packQty,
            PackUnitFa = packUnit,
            MeasurementFa = measurement,
            PriceToman = priceToman,
        };

        return true;
    }

    private static string ExtractExternalCode(IReadOnlyList<string> chunk)
    {
        foreach (var line in chunk)
        {
            var match = ExternalCodeRegex.Match(line);
            if (match.Success)
                return match.Groups[1].Value;
        }

        var joined = string.Join(" ", chunk);
        var joinedMatch = ExternalCodeRegex.Match(joined);
        if (joinedMatch.Success)
            return joinedMatch.Groups[1].Value;

        foreach (var line in chunk)
        {
            var tailMatch = TailLineRegex.Match(line);
            if (tailMatch.Success)
                return tailMatch.Groups[1].Value;
        }

        return string.Empty;
    }

    private static bool TryParsePriceTail(
        IReadOnlyList<string> chunk,
        string externalCode,
        out string packQty,
        out string packUnit,
        out decimal priceToman)
    {
        packQty = string.Empty;
        packUnit = string.Empty;
        priceToman = 0;

        for (var i = chunk.Count - 1; i >= 0; i--)
        {
            var line = chunk[i];
            var tailMatch = TailLineRegex.Match(line);
            if (tailMatch.Success && tailMatch.Groups[1].Value == externalCode)
            {
                packQty = tailMatch.Groups[2].Value.Trim();
                packUnit = tailMatch.Groups[3].Value.Trim();
                priceToman = ParseRialToToman(tailMatch.Groups[4].Value);
                return priceToman > 0;
            }
        }

        var priceIndex = -1;
        for (var i = chunk.Count - 1; i >= 0; i--)
        {
            var priceMatch = PriceTokenRegex.Match(chunk[i]);
            if (!priceMatch.Success)
                continue;

            priceToman = ParseRialToToman(priceMatch.Groups[1].Value);
            if (priceToman > 0)
            {
                priceIndex = i;
                break;
            }
        }

        if (priceIndex < 0)
            return false;

        var metaParts = new List<string>();
        for (var i = Math.Max(0, priceIndex - 3); i < priceIndex; i++)
            metaParts.Add(chunk[i]);

        var metaLine = string.Join(" ", metaParts).Replace('\t', ' ');
        metaLine = Regex.Replace(metaLine, @"\s+", " ").Trim();

        var metaMatch = Regex.Match(
            metaLine,
            $@"{Regex.Escape(externalCode)}[\t\s]+(\d+)[\t\s]+(.+?)$");
        if (metaMatch.Success)
        {
            packQty = metaMatch.Groups[1].Value.Trim();
            packUnit = metaMatch.Groups[2].Value.Trim();
            return true;
        }

        var looseMatch = Regex.Match(
            metaLine,
            @"(\d+)[\t\s]+([\p{L}\p{Nd}\s]+)$");
        if (looseMatch.Success)
        {
            packQty = looseMatch.Groups[1].Value.Trim();
            packUnit = looseMatch.Groups[2].Value.Trim();
            return true;
        }

        return false;
    }

    private static decimal ParseRialToToman(string raw)
    {
        var digits = raw.Replace(",", string.Empty, StringComparison.Ordinal).Trim();
        if (!decimal.TryParse(digits, NumberStyles.Number, CultureInfo.InvariantCulture, out var rial))
            return 0;
        return Math.Round(rial / 10m, 0, MidpointRounding.AwayFromZero);
    }

    private static string BuildTitle(IReadOnlyList<string> chunk, string externalCode)
    {
        var titleLines = new List<string>();
        foreach (var line in chunk)
        {
            if (line.Contains('(') && line.Contains(')'))
                break;
            if (Regex.IsMatch(line, @"^\d{4,7}[\t\s]"))
                break;
            if (PriceTokenRegex.IsMatch(line) && Regex.IsMatch(line, @"^\d{4,7}[\t\s]"))
                break;

            var cleaned = line.Replace('\t', ' ').Trim();
            if (cleaned.Length == 0)
                continue;
            if (Regex.IsMatch(cleaned, @"^\d+\s*عددی$"))
                continue;
            if (cleaned is "عددی" or "سی" or "سی سی")
                continue;

            titleLines.Add(cleaned);
        }

        var title = string.Join(" ", titleLines).Trim();
        title = Regex.Replace(title, @"\s+", " ");
        title = Regex.Replace(title, @"\(\s*" + Regex.Escape(externalCode) + @"\s*\)", string.Empty).Trim();
        return title;
    }

    private static string ExtractMeasurement(IReadOnlyList<string> chunk, string externalCode)
    {
        foreach (var line in chunk)
        {
            if (line.Contains('(') && line.Contains(externalCode, StringComparison.Ordinal))
            {
                var before = line.Split('(')[0].Trim();
                before = Regex.Replace(before, @"\b(sb|gb|sl)\b", string.Empty, RegexOptions.IgnoreCase).Trim();
                before = Regex.Replace(before, @"\s+", " ").Trim(' ', '\t', '-');
                if (before.Length > 0)
                    return before;
            }
        }

        var joined = string.Join(" ", chunk);
        var parenIndex = joined.IndexOf('(');
        if (parenIndex > 0 && joined.Contains(externalCode, StringComparison.Ordinal))
        {
            var before = joined[..parenIndex].Trim();
            before = Regex.Replace(before, @"\b(sb|gb|sl)\b", string.Empty, RegexOptions.IgnoreCase).Trim();
            before = Regex.Replace(before, @"\s+", " ").Trim(' ', '\t', '-');
            if (before.Length > 0 && before.Any(ch => ch >= '\u0600'))
                return before;
        }

        return string.Empty;
    }

    private static void AssignProductCodes(List<ParsedInventoryProduct> products)
    {
        var externalCounts = products
            .GroupBy(p => p.ExternalCode, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);

        foreach (var product in products)
        {
            if (externalCounts[product.ExternalCode] == 1 && product.ExternalCode.Length <= 10)
            {
                product.ProductCode = product.ExternalCode;
                continue;
            }

            var suffixLength = Math.Max(1, 10 - product.ExternalCode.Length);
            var suffix = product.Barcode[^Math.Min(suffixLength, product.Barcode.Length)..];
            product.ProductCode = (product.ExternalCode + suffix)[..Math.Min(10, product.ExternalCode.Length + suffix.Length)];
        }
    }
}
