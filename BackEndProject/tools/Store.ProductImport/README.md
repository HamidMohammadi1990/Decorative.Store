# Product catalog PDF import

Imports products from inventory / price-list PDFs into `DecorativeStoreDB`.

## Supported formats

### Belza price lists (`Store Files/*.pdf`)

Columns: row, product name, product code, carton quantity, sale price (purchase), consumer price (selling).

- **Product code** → `Product.ProductCode`
- **Consumer price** → `Product.Price` (۰ if empty / توقف تولید)
- **Sale price** → stored as feature **قیمت خرید** (not used as selling price)
- **Carton quantity** → property + feature **تعداد در کارتن**
- **Subcategory** → inferred from product name (bowls, plates, serveware, …)

### Pasabahce inventory PDF (legacy)

Barcode-based report with images — uses the original parser.

## Usage

```powershell
cd BackEndProject

# Parse all 15 PDFs in Store Files (default folder)
dotnet run --project tools/Store.ProductImport -- --parse-only

# Preview JSON export
dotnet run --project tools/Store.ProductImport -- --parse-only --export=tools/parsed-belza.json

# Dry-run against DB (validates subcategories, no writes)
dotnet run --project tools/Store.ProductImport -- --dry-run

# Full import from Store Files folder
dotnet run --project tools/Store.ProductImport

# Custom folder
dotnet run --project tools/Store.ProductImport -- --folder="D:\Projects\DecorativeStore\Store Files"

# Single PDF
dotnet run --project tools/Store.ProductImport -- "..\Store Files\تاینی.pdf" --parse-only
```

Uses `appsettings.Development.json` connection string. Skips rows when `ProductCode` already exists.

Ensure SQL Server is running and categories are seeded before import.
