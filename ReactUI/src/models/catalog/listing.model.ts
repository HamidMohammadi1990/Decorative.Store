import type { ProductSummary } from '@/models/catalog/product.model'

export type FilterFacetType = 'checkbox' | 'color' | 'range'

export interface FilterOption {
  value: string
  label: string
  count: number
  swatch?: string
}

export interface FilterFacet {
  id: string
  label: string
  type: FilterFacetType
  options: FilterOption[]
  range?: {
    min: number
    max: number
    step: number
    selectedMin?: number
    selectedMax?: number
  }
}

export type SortOptionId = 'featured' | 'price-asc' | 'price-desc' | 'newest'

export interface SortOption {
  id: SortOptionId
  label: string
}

export interface ListingBreadcrumb {
  label: string
  href: string
}

export interface ProductListingResult {
  title: string
  description?: string
  breadcrumbs: ListingBreadcrumb[]
  products: ProductSummary[]
  facets: FilterFacet[]
  sortOptions: SortOption[]
  totalCount: number
  page: number
  pageSize: number
  activeFilters: Record<string, string[]>
  pathNotFound?: boolean
}

export interface ListingQuery {
  pathname: string
  searchParams: URLSearchParams
}
