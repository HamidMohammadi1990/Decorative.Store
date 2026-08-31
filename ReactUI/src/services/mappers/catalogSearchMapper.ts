import type { CatalogSearchProduct } from '@/models/catalog/catalogSearch.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import { mapCatalogListingProduct } from '@/services/mappers/catalogListingMapper'

export function mapCatalogSearchProduct(product: CatalogSearchProduct): ProductSummary {
  return mapCatalogListingProduct({
    id: product.id,
    title: product.title,
    slug: product.slug,
    imageUrl: product.imageUrl,
    imageAlt: product.imageAlt,
    price: product.price,
    currencyCode: product.currencyCode,
    compareAtPrice: product.compareAtPrice,
    inStock: product.inStock,
    onSale: product.onSale,
    isNew: product.isNew,
    categorySlug: product.categorySlug,
    subCategorySlug: product.subCategorySlug,
  })
}
