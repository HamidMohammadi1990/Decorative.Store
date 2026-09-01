import { CMS_SECTION_TYPES } from '@/constants/cmsSectionTypes'
import type { CmsPage, CmsPageSection } from '@/models/cms/cmsPage.model'
import type { CmsContentPageContent } from '@/models/content/cmsContentPage.model'
import type { AppLink } from '@/models/shared/link.model'

function toLink(label: string, href: string): AppLink {
  return { label, href: href || '/' }
}

function parseEncodedDescription(description: string | null | undefined) {
  const raw = description?.trim() ?? ''
  let eyebrow: string | undefined
  let subtitle: string | undefined
  const textParts: string[] = []

  for (const part of raw.split('|')) {
    const trimmed = part.trim()
    if (!trimmed) continue
    if (trimmed.startsWith('eyebrow:')) {
      eyebrow = trimmed.slice('eyebrow:'.length).trim()
      continue
    }
    if (trimmed.startsWith('cta:')) continue
    textParts.push(trimmed)
  }

  if (textParts.length > 0 && !subtitle) {
    subtitle = textParts.join('|')
  }

  return { eyebrow, subtitle }
}

function resolveSectionTypeKey(sectionTypeName: string): string {
  const trimmed = sectionTypeName.trim()
  if (!trimmed) return ''
  const known = Object.values(CMS_SECTION_TYPES) as string[]
  return known.find((key) => key.toLowerCase() === trimmed.toLowerCase()) ?? trimmed
}

function mapContentHero(section: CmsPageSection): CmsContentPageContent['hero'] {
  const parsed = parseEncodedDescription(section.description)
  return {
    eyebrow: parsed.eyebrow,
    title: section.title,
    subtitle: parsed.subtitle,
  }
}

function mapContentBodyBlock(section: CmsPageSection): CmsContentPageContent['blocks'][number] {
  return {
    heading: section.title,
    lead: section.description ?? undefined,
    paragraphs: section.items
      .filter((item) => item.icon === 'paragraph')
      .map((item) => item.description?.trim() ?? '')
      .filter(Boolean),
  }
}

function mapContentSteps(section: CmsPageSection): CmsContentPageContent['steps'] {
  return {
    heading: section.title,
    items: section.items
      .filter((item) => item.icon === 'step')
      .map((item) => ({
        title: item.title,
        description: item.description ?? '',
      })),
  }
}

function mapContentCta(section: CmsPageSection): CmsContentPageContent['cta'] {
  const primary = section.items.find((item) => item.icon === 'cta')
  const secondary = section.items.find((item) => item.icon === 'cta-secondary')
  return {
    title: section.title,
    subtitle: section.description ?? undefined,
    primaryCta: toLink(primary?.title ?? 'Learn more', section.url || primary?.url || '/'),
    secondaryCta: secondary?.title
      ? toLink(secondary.title, secondary.url || '/')
      : undefined,
  }
}

const emptyContentPage = (slug: string): CmsContentPageContent => ({
  slug,
  title: '',
  hero: { title: '' },
  blocks: [],
  steps: { heading: '', items: [] },
})

export function mapCmsPageToContentPage(page: CmsPage): CmsContentPageContent {
  const content = emptyContentPage(page.slug)
  content.slug = page.slug
  content.title = page.title
  content.metaTitle = page.metaTitle
  content.metaDescription = page.metaDescription

  for (const section of page.sections) {
    const typeName = resolveSectionTypeKey(section.sectionTypeName)
    switch (typeName) {
      case CMS_SECTION_TYPES.contentHero:
        content.hero = mapContentHero(section)
        break
      case CMS_SECTION_TYPES.contentBodyBlock:
        content.blocks.push(mapContentBodyBlock(section))
        break
      case CMS_SECTION_TYPES.contentStepsGrid:
        content.steps = mapContentSteps(section)
        break
      case CMS_SECTION_TYPES.contentCtaStrip:
        content.cta = mapContentCta(section)
        break
      default:
        break
    }
  }

  return content
}
