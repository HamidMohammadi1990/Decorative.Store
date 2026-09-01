export interface CatalogListingBreadcrumb {
  label: string
  href: string
}

export interface CatalogListingFacetOption {
  value: string
  label: string
  count: number
  swatch?: string | null
}

export interface CatalogListingFacetGroup {
  id: string
  label: string
  type: 'checkbox' | 'color' | 'range'
  options: CatalogListingFacetOption[]
  range?: {
    min: number
    max: number
    step: number
    selectedMin?: number
    selectedMax?: number
  }
}

export interface CatalogListingProduct {
  id: string
  title: string
  slug: string
  imageUrl: string
  imageAlt: string
  price: number
  currencyCode: string
  compareAtPrice?: number
  inStock: boolean
  onSale: boolean
  isNew: boolean
  categorySlug: string
  subCategorySlug: string
  facets?: Record<string, string[]>
  reviewCount?: number
  averageRating?: number | null
  purchaseCount?: number
}

export interface CatalogListingResponse {
  title: string
  pathNotFound: boolean
  breadcrumbs: CatalogListingBreadcrumb[]
  products: CatalogListingProduct[]
  totalCount: number
  page: number
  pageSize: number
  facetGroups: CatalogListingFacetGroup[]
}

export interface CatalogListingQueryParams {
  path: string
  searchParams: URLSearchParams
}
