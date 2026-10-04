import type { AppLink } from '@/models/shared/link.model'
import type { ImageAsset } from '@/models/shared/image.model'

export interface AboutHero {
  eyebrow?: string
  title: string
  subtitle?: string
  image: ImageAsset
  cta: AppLink
}

export interface AboutStory {
  heading: string
  lead: string
  paragraphs: string[]
  image: ImageAsset
}

export interface AboutStat {
  value: string
  label: string
}

export interface AboutValue {
  id: string
  title: string
  description: string
}

export interface AboutTimelineItem {
  year: string
  text: string
}

export interface AboutCta {
  title: string
  subtitle?: string
  cta: AppLink
}

export interface AboutPageContent {
  slug: string
  title: string
  metaTitle?: string | null
  metaDescription?: string | null
  hero: AboutHero
  story: AboutStory
  stats: { heading: string; items: AboutStat[] }
  values: { heading: string; items: AboutValue[] }
  timeline: { heading: string; items: AboutTimelineItem[] }
  cta: AboutCta
}
