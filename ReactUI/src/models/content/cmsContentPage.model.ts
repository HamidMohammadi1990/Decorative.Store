import type { AppLink } from '@/models/shared/link.model'

export interface ContentHero {
  eyebrow?: string
  title: string
  subtitle?: string
}

export interface ContentBodyBlock {
  heading: string
  lead?: string
  paragraphs: string[]
}

export interface ContentStep {
  title: string
  description: string
}

export interface ContentCta {
  title: string
  subtitle?: string
  primaryCta: AppLink
  secondaryCta?: AppLink
}

export interface CmsContentPageContent {
  slug: string
  title: string
  metaTitle?: string | null
  metaDescription?: string | null
  hero: ContentHero
  blocks: ContentBodyBlock[]
  steps: { heading: string; items: ContentStep[] }
  cta?: ContentCta
}
