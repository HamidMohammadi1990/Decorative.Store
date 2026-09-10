# Pasabahce inventory PDF import

Imports products from the inventory PDF report into `DecorativeStoreDB`.

## What it does

- Parses ~1038 products from the PDF (title, external code, pack quantity, measurement, price)
- Converts **Rial → Toman** (÷10)
- Skips rows when `ProductCode` already exists (no duplicates)
- Creates FA + EN translations, slugs, short + long descriptions
- Assigns subcategory from product name (mugs, glassware, sets, …)
- Adds properties: **تعداد در بسته**, **واحد اندازه‌گیری**, plus `material=glass`
- Extracts product images from PDF pages when available
- Saves images to `Store.Api/wwwroot/Uploads/Products/`

## Usage

```powershell
cd BackEndProject

# Parse PDF only (no database connection)
dotnet run --project tools/Store.ProductImport -- --parse-only --export=tools/parsed-products.json

# Dry-run against DB (validates subcategories, skips writes)
dotnet run --project tools/Store.ProductImport -- --dry-run

# Export parsed JSON preview
dotnet run --project tools/Store.ProductImport -- --dry-run --export=tools/parsed-products.json

# Full import (uses appsettings.Development.json connection string)
dotnet run --project tools/Store.ProductImport

# Custom PDF path
dotnet run --project tools/Store.ProductImport -- "C:\path\to\report.pdf"
```

Ensure SQL Server is running and categories are already seeded before import.
