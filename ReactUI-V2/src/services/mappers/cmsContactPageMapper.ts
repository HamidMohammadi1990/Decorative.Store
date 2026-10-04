import {
  CMS_SECTION_TYPES,
} from '@/constants/cmsSectionTypes'
import type { CmsPage, CmsPageSection } from '@/models/cms/cmsPage.model'
import type { ContactPageContent } from '@/models/contact/contactPage.model'

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

function mapContactHero(section: CmsPageSection): ContactPageContent['hero'] {
  const parsed = parseEncodedDescription(section.description)
  return {
    eyebrow: parsed.eyebrow,
    title: section.title,
    subtitle: parsed.subtitle,
  }
}

function mapContactMethods(section: CmsPageSection): ContactPageContent['methods'] {
  return {
    heading: section.title,
    items: section.items.map((item) => ({
      id: item.icon ?? item.title,
      title: item.title,
      description: item.description ?? '',
      href: item.url ?? '',
    })),
  }
}

function splitLocationDescription(description: string | null | undefined) {
  const parts = (description ?? '').split('|').map((part) => part.trim()).filter(Boolean)
  return {
    address: parts[0] ?? '',
    hours: parts[1] ?? '',
  }
}

function mapContactLocations(section: CmsPageSection): ContactPageContent['locations'] {
  return {
    heading: section.title,
    items: section.items.map((item) => {
      const { address, hours } = splitLocationDescription(item.description)
      return {
        title: item.title,
        address,
        hours,
        mapHref: item.url ?? '',
      }
    }),
  }
}

function mapContactFormIntro(section: CmsPageSection): ContactPageContent['formIntro'] {
  const noteItem = section.items.find((item) => item.icon === 'note')
  return {
    heading: section.title,
    lead: section.description ?? undefined,
    email: section.url?.trim() || 'support@dibagallery.com',
    note: noteItem?.description?.trim() || undefined,
  }
}

const emptyContactPage = (): ContactPageContent => ({
  slug: 'contact',
  title: 'Contact Us',
  hero: { title: '' },
  methods: { heading: '', items: [] },
  locations: { heading: '', items: [] },
  formIntro: { heading: '', email: '' },
})

export function mapCmsPageToContactPage(page: CmsPage): ContactPageContent {
  const content = emptyContactPage()
  content.slug = page.slug
  content.title = page.title
  content.metaTitle = page.metaTitle
  content.metaDescription = page.metaDescription

  for (const section of page.sections) {
    const typeName = resolveSectionTypeKey(section.sectionTypeName)
    switch (typeName) {
      case CMS_SECTION_TYPES.contactHero:
        content.hero = mapContactHero(section)
        break
      case CMS_SECTION_TYPES.contactMethodsGrid:
        content.methods = mapContactMethods(section)
        break
      case CMS_SECTION_TYPES.contactLocationsGrid:
        content.locations = mapContactLocations(section)
        break
      case CMS_SECTION_TYPES.contactFormIntro:
        content.formIntro = mapContactFormIntro(section)
        break
      default:
        break
    }
  }

  return content
}
