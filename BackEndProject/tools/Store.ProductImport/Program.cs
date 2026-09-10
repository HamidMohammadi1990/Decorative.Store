using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store.Infrastructure.Persistence;
using Store.ProductImport.Import;
using Store.ProductImport.Parsing;
using System.Text;
using System.Text.Json;

const string defaultPdfPath = @"c:\Users\40312758\Downloads\__گزارش موجودی کالا با عکس و قیمت_.pdf";

var pdfPath = args.FirstOrDefault(arg => !arg.StartsWith("--", StringComparison.Ordinal)) ?? defaultPdfPath;
var dryRun = args.Any(arg => arg.Equals("--dry-run", StringComparison.OrdinalIgnoreCase));
var parseOnly = args.Any(arg => arg.Equals("--parse-only", StringComparison.OrdinalIgnoreCase));
var exportJson = args.FirstOrDefault(arg => arg.StartsWith("--export=", StringComparison.OrdinalIgnoreCase));
var dumpText = args.FirstOrDefault(arg => arg.StartsWith("--dump-text=", StringComparison.OrdinalIgnoreCase));

if (!File.Exists(pdfPath))
{
    Console.Error.WriteLine($"PDF not found: {pdfPath}");
    return 1;
}

Console.WriteLine($"Parsing PDF: {pdfPath}");

if (!string.IsNullOrWhiteSpace(dumpText))
{
    var dumpPath = dumpText["--dump-text=".Length..];
    var rawText = InventoryPdfParser.ExtractText(pdfPath);
    await File.WriteAllTextAsync(dumpPath, rawText, Encoding.UTF8);
    Console.WriteLine($"Dumped raw PDF text ({rawText.Length} chars): {dumpPath}");
}

var products = InventoryPdfParser.ParsePdf(pdfPath);
Console.WriteLine($"Parsed products: {products.Count}");
Console.WriteLine($"With images: {products.Count(p => p.ImageBytes is { Length: > 0 })}");

if (products.Count == 0)
{
    Console.WriteLine("No products parsed. Dump raw text to inspect layout:");
    Console.WriteLine("  dotnet run --project tools/Store.ProductImport -- --dump-text=tools/pdf-raw.txt --parse-only");
}
else if (products.Count > 0)
{
    var sample = products.Take(3).Select(p => $"  #{p.Row} {p.TitleFa} | code={p.ExternalCode} | {p.PriceToman:N0} T");
    Console.WriteLine("Sample:");
    foreach (var line in sample)
        Console.WriteLine(line);
}

if (!string.IsNullOrWhiteSpace(exportJson))
{
    var exportPath = exportJson["--export=".Length..];
    var payload = products.Select(p => new
    {
        p.Row,
        p.Barcode,
        p.ProductCode,
        p.ExternalCode,
        p.TitleFa,
        p.TitleEn,
        p.SlugFa,
        p.SlugEn,
        p.PriceToman,
        p.PackQuantity,
        p.PackUnitFa,
        p.MeasurementFa,
        p.SubCategorySlug,
        HasImage = p.ImageBytes is { Length: > 0 },
    });
    await File.WriteAllTextAsync(exportPath, JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine($"Exported preview JSON: {exportPath}");
}

if (parseOnly)
{
    Console.WriteLine("Parse-only mode — skipping database import.");
    return 0;
}

var apiSettingsPath = Path.GetFullPath(Path.Combine(
    AppContext.BaseDirectory,
    "..", "..", "..",
    "src", "Presentation", "Store.Api", "appsettings.Development.json"));
if (!File.Exists(apiSettingsPath))
{
    apiSettingsPath = Path.GetFullPath(Path.Combine(
        Directory.GetCurrentDirectory(),
        "src", "Presentation", "Store.Api", "appsettings.Development.json"));
}

if (!File.Exists(apiSettingsPath))
{
    Console.Error.WriteLine($"Could not locate appsettings.Development.json near: {apiSettingsPath}");
    if (dryRun)
        return 0;
    return 1;
}

var configuration = new ConfigurationBuilder()
    .AddJsonFile(apiSettingsPath, optional: false)
    .Build();

var connectionString = configuration.GetConnectionString("EditionDbContext");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("EditionDbContext connection string is missing.");
    return 1;
}

var options = new DbContextOptionsBuilder<EditionDbContext>()
    .UseSqlServer(connectionString)
    .Options;

var uploadsRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(apiSettingsPath)!, "wwwroot"));

await using var db = new EditionDbContext(options);
var importer = new ProductImportService(db, uploadsRoot);

Console.WriteLine(dryRun ? "Dry run — no database writes." : "Importing into database...");
var summary = await importer.ImportAsync(products, dryRun);

Console.WriteLine($"Imported: {summary.Imported}");
Console.WriteLine($"Skipped (duplicate ProductCode): {summary.SkippedDuplicate}");
Console.WriteLine($"Failed: {summary.Failed}");

if (summary.Errors.Count > 0)
{
    Console.WriteLine("Errors:");
    foreach (var error in summary.Errors.Take(20))
        Console.WriteLine($"  - {error}");
    if (summary.Errors.Count > 20)
        Console.WriteLine($"  ... and {summary.Errors.Count - 20} more");
}

return summary.Failed > 0 ? 2 : 0;
