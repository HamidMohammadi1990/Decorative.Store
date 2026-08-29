import type { ImageAsset } from '@/models/shared/image.model'
import type { Money } from '@/models/shared/money.model'

export interface ProductFacets {
  color?: string[]
  size?: string[]
  material?: string[]
  room?: string[]
}

export interface ProductSummary {
  id: string
  slug: string
  title: string
  image: ImageAsset
  price: Money
  compareAtPrice?: Money
  categorySlugs: string[]
  subcategorySlug?: string
  facets: ProductFacets
  badges: string[]
  inStock: boolean
  onSale: boolean
  isNew: boolean
  reviewCount: number
  averageRating?: number
  satisfactionPercent?: number
  purchaseCount: number
}
