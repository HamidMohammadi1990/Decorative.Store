import type { ImageAsset } from '@/models/shared/image.model'
import type { AppLink } from '@/models/shared/link.model'

export interface PromoTile {
  id: string
  title: string
  subtitle?: string
  image: ImageAsset
  link: AppLink
}

export interface PromoTileStrip {
  tiles: PromoTile[]
  disclaimer?: string
  disclaimerLink?: AppLink
}
