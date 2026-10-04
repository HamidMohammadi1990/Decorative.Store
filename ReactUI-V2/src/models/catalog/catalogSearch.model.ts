export interface CatalogSearchCategory {
  title: string
  slug: string
}

export interface CatalogSearchSubCategory {
  title: string
  slug: string
  categoryTitle: string
  categorySlug: string
}

export interface CatalogSearchProduct {
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

export interface CatalogSearchResponse {
  categories: CatalogSearchCategory[]
  subCategories: CatalogSearchSubCategory[]
  products: CatalogSearchProduct[]
}
