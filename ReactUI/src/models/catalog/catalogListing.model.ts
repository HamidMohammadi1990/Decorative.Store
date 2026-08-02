export interface CatalogListingBreadcrumb {
  label: string
  href: string
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
}

export interface CatalogListingResponse {
  title: string
  pathNotFound: boolean
  breadcrumbs: CatalogListingBreadcrumb[]
  products: CatalogListingProduct[]
}
