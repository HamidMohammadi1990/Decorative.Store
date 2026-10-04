import type { ImageAsset } from '@/models/shared/image.model'
import type { AppLink } from '@/models/shared/link.model'

export interface FeaturedShopCard {
  id: string
  title: string
  subtitle?: string
  image: ImageAsset
  link: AppLink
  links?: AppLink[]
}

export interface FeaturedShopGrid {
  sections: FeaturedShopCard[]
}
