import type { ImageAsset } from '@/models/shared/image.model'
import type { ProductSummary } from '@/models/catalog/product.model'

export interface ProductFeature {
  label: string
  value: string
}

export interface ProductFeatureGroup {
  id: string
  title: string
  features: ProductFeature[]
}

export interface ProductDetail extends ProductSummary {
  description: string
  longDescription: string[]
  highlights: string[]
  images: ImageAsset[]
  features: ProductFeature[]
  featureGroups: ProductFeatureGroup[]
  dimensions?: string
  care?: string
  deliveryNote?: string
  warranty?: string
}
