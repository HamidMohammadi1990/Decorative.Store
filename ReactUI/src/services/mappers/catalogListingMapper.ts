import { API_BASE_URL } from '@/config/api'
import type { CatalogListingProduct } from '@/models/catalog/catalogListing.model'
import type { ProductSummary } from '@/models/catalog/product.model'

function resolveProductImageSrc(imageUrl: string): string {
  if (!imageUrl) return ''
  if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl
  return `${API_BASE_URL}${imageUrl.startsWith('/') ? imageUrl : `/${imageUrl}`}`
}

function toSubcategorySegment(subCategorySlug: string): string | undefined {
  if (!subCategorySlug) return undefined
  const parts = subCategorySlug.split('/').filter(Boolean)
  return parts[parts.length - 1]
}

function toCategorySegments(categorySlug: string, subCategorySlug: string): string[] {
  const segments = new Set<string>()
  if (categorySlug) segments.add(categorySlug)
  if (subCategorySlug) {
    subCategorySlug
      .split('/')
      .filter(Boolean)
      .forEach((segment) => segments.add(segment))
  }
  return [...segments]
}

export function mapCatalogListingProduct(product: CatalogListingProduct): ProductSummary {
  return {
    id: product.id,
    slug: product.slug,
    title: product.title,
    image: {
      src:
        resolveProductImageSrc(product.imageUrl) ||
        '/images/home/new-arrivals.svg',
      alt: product.imageAlt || product.title,
    },
    price: {
      amount: product.price,
      currencyCode: product.currencyCode,
    },
    compareAtPrice: product.compareAtPrice
      ? {
          amount: product.compareAtPrice,
          currencyCode: product.currencyCode,
        }
      : undefined,
    categorySlugs: toCategorySegments(product.categorySlug, product.subCategorySlug),
    subcategorySlug: toSubcategorySegment(product.subCategorySlug),
    facets: {},
    badges: [],
    inStock: product.inStock,
    onSale: product.onSale,
    isNew: product.isNew,
    reviewCount: 0,
    purchaseCount: 0,
  }
}
