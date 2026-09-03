import {
  CMS_SECTION_TYPES,
} from '@/constants/cmsSectionTypes'
import type { CmsPage, CmsPageSection } from '@/models/cms/cmsPage.model'
import type { AboutPageContent } from '@/models/about/aboutPage.model'
import type { AppLink } from '@/models/shared/link.model'
import type { ImageAsset } from '@/models/shared/image.model'
import { resolveCmsImageSrc } from '@/services/adminCmsMediaService'

function toLink(label: string, href: string): AppLink {
  return { label, href: href || '/' }
}

function toImage(src: string | null | undefined, alt: string): ImageAsset {
  const trimmed = src?.trim()
  if (!trimmed) return { src: '/images/home/living-room.jpg', alt }
  return { src: resolveCmsImageSrc(trimmed), alt }
}

function findItemByIcon(section: CmsPageSection, icon: string) {
  return section.items.find((item) => item.icon === icon)
}

function parseEncodedDescription(description: string | null | undefined) {
  const raw = description?.trim() ?? ''
  let eyebrow: string | undefined
  let subtitle: string | undefined
  let ctaLabel: string | undefined
  let imageAlt: string | undefined
  const textParts: string[] = []

  for (const part of raw.split('|')) {
    const trimmed = part.trim()
    if (!trimmed) continue
    if (trimmed.startsWith('eyebrow:')) {
      eyebrow = trimmed.slice('eyebrow:'.length).trim()
      continue
    }
    if (trimmed.startsWith('cta:')) {
      ctaLabel = trimmed.slice('cta:'.length).trim()
      continue
    }
    if (trimmed.startsWith('alt:')) {
      imageAlt = trimmed.slice('alt:'.length).trim()
      continue
    }
    textParts.push(trimmed)
  }

  if (textParts.length > 0 && !subtitle) {
    subtitle = textParts.join('|')
  }

  return { eyebrow, subtitle, ctaLabel, imageAlt }
}

function resolveSectionTypeKey(sectionTypeName: string): string {
  const trimmed = sectionTypeName.trim()
  if (!trimmed) return ''
  const known = Object.values(CMS_SECTION_TYPES) as string[]
  return known.find((key) => key.toLowerCase() === trimmed.toLowerCase()) ?? trimmed
}

function mapAboutHero(section: CmsPageSection): AboutPageContent['hero'] {
  const parsed = parseEncodedDescription(section.description)
  const ctaItem = findItemByIcon(section, 'cta')
  return {
    eyebrow: parsed.eyebrow,
    title: section.title,
    subtitle: parsed.subtitle,
    image: toImage(section.imageUrl, parsed.imageAlt ?? section.title),
    cta: toLink(ctaItem?.title ?? parsed.ctaLabel ?? 'Learn more', section.url || ctaItem?.url || '/'),
  }
}

function mapAboutStory(section: CmsPageSection): AboutPageContent['story'] {
  return {
    heading: section.title,
    lead: section.description ?? '',
    paragraphs: section.items
      .filter((item) => item.icon === 'paragraph')
      .map((item) => item.description?.trim() ?? '')
      .filter(Boolean),
    image: toImage(section.imageUrl, section.title),
  }
}

function mapAboutStats(section: CmsPageSection): AboutPageContent['stats'] {
  return {
    heading: section.title,
    items: section.items
      .filter((item) => item.icon === 'stat')
      .map((item) => ({
        value: item.title,
        label: item.description ?? '',
      })),
  }
}

function mapAboutValues(section: CmsPageSection): AboutPageContent['values'] {
  return {
    heading: section.title,
    items: section.items.map((item) => ({
      id: item.icon ?? item.title,
      title: item.title,
      description: item.description ?? '',
    })),
  }
}

function mapAboutTimeline(section: CmsPageSection): AboutPageContent['timeline'] {
  return {
    heading: section.title,
    items: section.items
      .filter((item) => item.icon === 'milestone')
      .map((item) => ({
        year: item.title,
        text: item.description ?? '',
      })),
  }
}

function mapAboutCta(section: CmsPageSection): AboutPageContent['cta'] {
  const ctaItem = findItemByIcon(section, 'cta')
  return {
    title: section.title,
    subtitle: section.description ?? undefined,
    cta: toLink(ctaItem?.title ?? 'Learn more', section.url || ctaItem?.url || '/'),
  }
}

const emptyAboutPage = (): AboutPageContent => ({
  slug: 'about',
  title: 'About Us',
  hero: {
    title: '',
    image: toImage(null, ''),
    cta: toLink('', '/'),
  },
  story: { heading: '', lead: '', paragraphs: [], image: toImage(null, '') },
  stats: { heading: '', items: [] },
  values: { heading: '', items: [] },
  timeline: { heading: '', items: [] },
  cta: { title: '', cta: toLink('', '/') },
})

export function mapCmsPageToAboutPage(page: CmsPage): AboutPageContent {
  const content = emptyAboutPage()
  content.slug = page.slug
  content.title = page.title
  content.metaTitle = page.metaTitle
  content.metaDescription = page.metaDescription

  for (const section of page.sections) {
    const typeName = resolveSectionTypeKey(section.sectionTypeName)
    switch (typeName) {
      case CMS_SECTION_TYPES.aboutHero:
        content.hero = mapAboutHero(section)
        break
      case CMS_SECTION_TYPES.aboutStoryBlock:
        content.story = mapAboutStory(section)
        break
      case CMS_SECTION_TYPES.aboutStatsStrip:
        content.stats = mapAboutStats(section)
        break
      case CMS_SECTION_TYPES.aboutValuesGrid:
        content.values = mapAboutValues(section)
        break
      case CMS_SECTION_TYPES.aboutTimeline:
        content.timeline = mapAboutTimeline(section)
        break
      case CMS_SECTION_TYPES.aboutCtaStrip:
        content.cta = mapAboutCta(section)
        break
      default:
        break
    }
  }

  return content
}
