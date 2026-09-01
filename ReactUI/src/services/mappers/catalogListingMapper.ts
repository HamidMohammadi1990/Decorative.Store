import { API_BASE_URL } from '@/config/api'
import type {
  CatalogListingFacetGroup,
  CatalogListingProduct,
  CatalogListingResponse,
} from '@/models/catalog/catalogListing.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import type { FilterFacet } from '@/models/catalog/listing.model'
import { CHECKBOX_FILTER_KEYS, LISTING_QUERY_RESERVED_KEYS } from '@/extensions/listingFilters'

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
    facets: product.facets ?? {},
    badges: [],
    inStock: product.inStock,
    onSale: product.onSale,
    isNew: product.isNew,
    reviewCount: product.reviewCount ?? 0,
    averageRating: product.averageRating ?? undefined,
    purchaseCount: product.purchaseCount ?? 0,
  }
}

export function mapCatalogListingFacetGroups(groups: CatalogListingFacetGroup[]): FilterFacet[] {
  return groups.map((group) => ({
    id: group.id,
    label: group.label,
    type: group.type,
    options: group.options.map((option) => ({
      value: option.value,
      label: option.label,
      count: option.count,
      swatch: option.swatch ?? undefined,
    })),
    range: group.range
      ? {
          min: group.range.min,
          max: group.range.max,
          step: group.range.step,
          selectedMin: group.range.selectedMin,
          selectedMax: group.range.selectedMax,
        }
      : undefined,
  }))
}

const RESERVED_LISTING_PARAMS = new Set([
  'path',
  'minPrice',
  'maxPrice',
  'inStock',
  'onSale',
  'isNew',
  'minRating',
  'sort',
  'page',
  'pageSize',
  ...LISTING_QUERY_RESERVED_KEYS,
])

export function buildCatalogListingQuery(searchParams: URLSearchParams): string {
  const parts: string[] = []

  const append = (key: string, value: string | number | boolean) => {
    parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`)
  }

  const minPrice = searchParams.get('minPrice')
  const maxPrice = searchParams.get('maxPrice')
  const sort = searchParams.get('sort')
  const page = searchParams.get('page')
  const pageSize = searchParams.get('pageSize')
  const minRating = searchParams.get('minRating')

  if (minPrice) append('minPrice', minPrice)
  if (maxPrice) append('maxPrice', maxPrice)
  if (sort && sort !== 'featured') append('sort', sort)
  if (page && page !== '1') append('page', page)
  if (pageSize) append('pageSize', pageSize)
  if (minRating) append('minRating', minRating)

  for (const key of CHECKBOX_FILTER_KEYS) {
    for (const value of searchParams.getAll(key)) {
      append(key, value)
    }
  }

  for (const [key, value] of searchParams.entries()) {
    if (RESERVED_LISTING_PARAMS.has(key) || CHECKBOX_FILTER_KEYS.includes(key as never))
      continue
    append(key, value)
  }

  return parts.length > 0 ? `&${parts.join('&')}` : ''
}
