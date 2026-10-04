export interface CatalogProductImage {
  url: string
  alt: string
}

export interface CatalogProductFeature {
  label: string
  value: string
  groupTitle?: string
}

export interface CatalogProductResponse {
  notFound: boolean
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
  description: string
  longDescriptions: string[]
  images: CatalogProductImage[]
  features: CatalogProductFeature[]
  reviewCount: number
  averageRating?: number
  satisfactionPercent?: number
  purchaseCount: number
}

export interface RelatedCatalogProductsResponse {
  products: import('@/models/catalog/catalogListing.model').CatalogListingProduct[]
}
