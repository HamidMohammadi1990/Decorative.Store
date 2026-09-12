using Store.ProductImport.Models;

namespace Store.ProductImport.Parsing;

public static class CatalogPdfParser
{
    public static IReadOnlyList<ParsedInventoryProduct> ParsePdf(string pdfPath)
    {
        var text = InventoryPdfParser.ExtractText(pdfPath);

        if (BelzaPriceListPdfParser.IsBelzaFormat(text))
            return BelzaPriceListPdfParser.ParsePdf(pdfPath);

        var pasabahce = InventoryPdfParser.ParsePdf(pdfPath);
        if (pasabahce.Count > 0)
            return pasabahce;

        // PdfPig may garble Persian headers — still try Belza row patterns (T-/SO/CO- codes).
        return BelzaPriceListPdfParser.ParsePdf(pdfPath);
    }

    public static IReadOnlyList<ParsedInventoryProduct> ParseFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

        var pdfFiles = Directory
            .GetFiles(folderPath, "*.pdf", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (pdfFiles.Count == 0)
            throw new InvalidOperationException($"No PDF files found in: {folderPath}");

        var allProducts = new List<ParsedInventoryProduct>();

        foreach (var pdfPath in pdfFiles)
        {
            var fileName = Path.GetFileName(pdfPath);
            Console.WriteLine($"  → {fileName}");
            var products = ParsePdf(pdfPath);
            Console.WriteLine($"     parsed {products.Count} product(s)");
            if (products.Count > 0)
            {
                var codes = string.Join(", ", products.Take(3).Select(p => p.ProductCode));
                Console.WriteLine($"     sample codes: {codes}{(products.Count > 3 ? ", …" : string.Empty)}");
            }
            allProducts.AddRange(products);
        }

        return allProducts
            .GroupBy(p => p.ProductCode, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList();
    }
}
