import type { ProductSummary } from '@/models/catalog/product.model'

export const CHECKBOX_FILTER_KEYS = [
  'color',
  'size',
  'material',
  'room',
  'inStock',
  'onSale',
  'isNew',
  'price',
] as const

/** Query params that are not product facet filters */
export const LISTING_QUERY_RESERVED_KEYS = new Set([
  'sort',
  'page',
  'pageSize',
  'q',
])

const SINGLE_VALUE_FILTER_KEYS = new Set(['minPrice', 'maxPrice', 'minRating'])

export type CheckboxFilterKey = (typeof CHECKBOX_FILTER_KEYS)[number]

export interface PriceBucket {
  id: string
  min: number
  max: number
}

export const PRICE_BUCKETS: PriceBucket[] = [
  { id: 'under-5m', min: 0, max: 4_999_999 },
  { id: '5m-10m', min: 5_000_000, max: 9_999_999 },
  { id: '10m-15m', min: 10_000_000, max: 14_999_999 },
  { id: '15m-20m', min: 15_000_000, max: 19_999_999 },
  { id: 'over-20m', min: 20_000_000, max: Number.POSITIVE_INFINITY },
]

export interface ParsedListingFilters {
  checkbox: Record<string, string[]>
  minPrice?: number
  maxPrice?: number
  minRating?: number
}

export function parseListingFilters(searchParams: URLSearchParams): ParsedListingFilters {
  const checkbox: Record<string, string[]> = {}

  for (const key of searchParams.keys()) {
    if (LISTING_QUERY_RESERVED_KEYS.has(key) || SINGLE_VALUE_FILTER_KEYS.has(key)) continue

    const values = searchParams.getAll(key).filter(Boolean)
    if (values.length > 0) checkbox[key] = values
  }

  const minRaw = searchParams.get('minPrice')
  const maxRaw = searchParams.get('maxPrice')
  const minRatingRaw = searchParams.get('minRating')
  const minPrice = minRaw != null && minRaw !== '' ? Number(minRaw) : undefined
  const maxPrice = maxRaw != null && maxRaw !== '' ? Number(maxRaw) : undefined
  const minRating =
    minRatingRaw != null && minRatingRaw !== '' ? Number(minRatingRaw) : undefined

  return {
    checkbox,
    minPrice: Number.isFinite(minPrice) ? minPrice : undefined,
    maxPrice: Number.isFinite(maxPrice) ? maxPrice : undefined,
    minRating: Number.isFinite(minRating) ? minRating : undefined,
  }
}

export function toActiveFiltersRecord(filters: ParsedListingFilters): Record<string, string[]> {
  const active: Record<string, string[]> = { ...filters.checkbox }
  if (filters.minPrice != null) active.minPrice = [String(filters.minPrice)]
  if (filters.maxPrice != null) active.maxPrice = [String(filters.maxPrice)]
  if (filters.minRating != null) active.minRating = [String(filters.minRating)]
  return active
}

function productMatchesCheckboxFacet(
  product: ProductSummary,
  facetId: string,
  values: string[],
) {
  if (facetId === 'inStock') {
    return values.includes('true') ? product.inStock : true
  }
  if (facetId === 'onSale') {
    return values.includes('true') ? product.onSale : true
  }
  if (facetId === 'isNew') {
    return values.includes('true') ? product.isNew : true
  }
  if (facetId === 'price') {
    return values.some((bucketId) => {
      const bucket = PRICE_BUCKETS.find((b) => b.id === bucketId)
      if (!bucket) return false
      return (
        product.price.amount >= bucket.min && product.price.amount <= bucket.max
      )
    })
  }

  const productValues = product.facets[facetId] ?? []
  const normalizedValues = values.map((v) => v.toLowerCase())
  return productValues.some((pv) => normalizedValues.includes(pv.toLowerCase()))
}

export function productMatchesPrice(
  product: ProductSummary,
  filters: ParsedListingFilters,
) {
  const bucketValues = filters.checkbox.price ?? []
  if (bucketValues.length > 0) {
    return bucketValues.some((bucketId) => {
      const bucket = PRICE_BUCKETS.find((b) => b.id === bucketId)
      if (!bucket) return false
      return (
        product.price.amount >= bucket.min && product.price.amount <= bucket.max
      )
    })
  }

  if (filters.minPrice != null && product.price.amount < filters.minPrice) {
    return false
  }
  if (filters.maxPrice != null && product.price.amount > filters.maxPrice) {
    return false
  }

  return true
}

export function productMatchesFilters(
  product: ProductSummary,
  filters: ParsedListingFilters,
  exceptFacetId?: string,
) {
  if (exceptFacetId !== 'price' && !productMatchesPrice(product, filters)) {
    return false
  }

  for (const [facetId, values] of Object.entries(filters.checkbox)) {
    if (facetId === exceptFacetId || values.length === 0) continue
    if (facetId === 'minRating') continue
    if (!productMatchesCheckboxFacet(product, facetId, values)) return false
  }

  if (exceptFacetId !== 'minRating' && filters.minRating != null) {
    if (product.averageRating == null || product.averageRating < filters.minRating) {
      return false
    }
  }

  return true
}

export function getProductPriceBounds(products: ProductSummary[]) {
  if (products.length === 0) return { min: 0, max: 0 }
  const amounts = products.map((p) => p.price.amount)
  return {
    min: Math.min(...amounts),
    max: Math.max(...amounts),
  }
}
