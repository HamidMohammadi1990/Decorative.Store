import type { ImageAsset } from '@/models/shared/image.model'
import type { AppLink } from '@/models/shared/link.model'

export interface HeroSlide {
  id: string
  eyebrow?: string
  title: string
  subtitle?: string
  image: ImageAsset
  cta: AppLink
}

export interface HeroCarousel {
  slides: HeroSlide[]
}
