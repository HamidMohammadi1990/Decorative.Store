import {
  CMS_CHROME_SECTION_TYPES,
  CMS_HOME_SECTION_TYPES,
  CMS_SECTION_TYPES,
} from '@/constants/cmsSectionTypes'
import type { CmsPage, CmsPageSection, CmsSectionItem } from '@/models/cms/cmsPage.model'
import type { HomePage } from '@/models/home/homePage.model'
import type { AppLink } from '@/models/shared/link.model'
import type { ImageAsset } from '@/models/shared/image.model'

function toLink(label: string, href: string): AppLink {
  return { label, href: href || '/' }
}
import { readRecord, readStringField } from '@/services/api/apiNormalize'
import { resolveCmsImageSrc } from '@/services/adminCmsMediaService'

function readNumberField(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'number' && Number.isFinite(value)) return value
  }
  return 0
}

function normalizeSectionItem(data: unknown): CmsSectionItem | null {
  const record = readRecord(data)
  if (!record) return null

  return {
    title: readStringField(record, 'title', 'Title'),
    priority: readNumberField(record, 'priority', 'Priority'),
    icon: readOptionalString(record, 'icon', 'Icon'),
    imageUrl: readOptionalString(record, 'imageUrl', 'ImageUrl'),
    url: readOptionalString(record, 'url', 'Url'),
    description: readOptionalString(record, 'description', 'Description'),
  }
}

function readOptionalString(record: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'string') return value
    if (value === null) return null
  }
  return undefined
}

function normalizeSection(data: unknown): CmsPageSection | null {
  const record = readRecord(data)
  if (!record) return null

  const itemsRaw = record.items ?? record.Items
  const items = Array.isArray(itemsRaw)
    ? itemsRaw
        .map(normalizeSectionItem)
        .filter((item): item is CmsSectionItem => item !== null)
    : []

  return {
    sectionTypeId: readNumberField(record, 'sectionTypeId', 'SectionTypeId'),
    sectionTypeName: readStringField(record, 'sectionTypeName', 'SectionTypeName'),
    priority: readNumberField(record, 'priority', 'Priority'),
    title: readStringField(record, 'title', 'Title'),
    description: readOptionalString(record, 'description', 'Description'),
    url: readStringField(record, 'url', 'Url'),
    imageUrl: readOptionalString(record, 'imageUrl', 'ImageUrl'),
    items,
  }
}

export function normalizeCmsPage(data: unknown): CmsPage | null {
  const record = readRecord(data)
  if (!record) return null

  const notFound = Boolean(record.notFound ?? record.NotFound)
  if (notFound) return null

  const sectionsRaw = record.sections ?? record.Sections
  const sections = Array.isArray(sectionsRaw)
    ? sectionsRaw
        .map(normalizeSection)
        .filter((section): section is CmsPageSection => section !== null)
    : []

  const slug = readStringField(record, 'slug', 'Slug')
  if (!slug) return null

  return {
    slug,
    title: readStringField(record, 'title', 'Title'),
    type: readNumberField(record, 'type', 'Type'),
    metaTitle: readOptionalString(record, 'metaTitle', 'MetaTitle'),
    metaDescription: readOptionalString(record, 'metaDescription', 'MetaDescription'),
    sections,
  }
}

function resolveSectionTypeKey(sectionTypeName: string): string {
  const trimmed = sectionTypeName.trim()
  if (!trimmed) return ''

  const known = Object.values(CMS_SECTION_TYPES) as string[]
  const match = known.find((key) => key.toLowerCase() === trimmed.toLowerCase())
  return match ?? trimmed
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

function emptyHomePage(): HomePage {
  return {
    promoAnnouncement: { id: 'default', message: '', link: { label: '', href: '/' } },
    utilityBar: { phoneLabel: '', phoneHref: '', links: [] },
    header: {
      brandLabel: '',
      searchPlaceholder: '',
      accountLabel: '',
      cartLabel: '',
      primaryNav: [],
    },
    designServices: { title: '', cta: { label: '', href: '/' }, featuredLinks: [] },
    hero: { slides: [] },
    promoTiles: { tiles: [] },
    categoryNav: { items: [] },
    featuredShop: { sections: [] },
    footer: {
      columns: [],
      newsletterTitle: '',
      newsletterPlaceholder: '',
      newsletterButton: '',
      copyright: '',
      legalLinks: [],
    },
  }
}

function mapPromoAnnouncement(section: CmsPageSection): HomePage['promoAnnouncement'] {
  const linkItem = section.items[0]
  return {
    id: section.title,
    message: section.description ?? section.title,
    link: toLink(linkItem?.title ?? 'Learn more', section.url || linkItem?.url || '/'),
  }
}

function mapUtilityBar(section: CmsPageSection): HomePage['utilityBar'] {
  return {
    phoneLabel: section.title,
    phoneHref: section.url,
    links: section.items.map((item) => toLink(item.title, item.url ?? '/')),
  }
}

function mapSiteHeader(section: CmsPageSection): HomePage['header'] {
  const search = findItemByIcon(section, 'search')
  const account = findItemByIcon(section, 'account')
  const cart = findItemByIcon(section, 'cart')

  return {
    brandLabel: section.title,
    searchPlaceholder: search?.title ?? '',
    accountLabel: account?.title ?? '',
    cartLabel: cart?.title ?? '',
    primaryNav: [],
  }
}

function mapDesignServices(section: CmsPageSection): HomePage['designServices'] {
  const ctaItem = findItemByIcon(section, 'cta') ?? section.items[0]
  const featuredLinks = section.items
    .filter((item) => item.icon !== 'cta')
    .map((item) => toLink(item.title, item.url ?? '/'))

  return {
    title: section.title,
    cta: toLink(ctaItem?.title ?? '', ctaItem?.url ?? '/'),
    featuredLinks,
  }
}

function mapHero(section: CmsPageSection): HomePage['hero'] {
  return {
    slides: section.items
      .filter((item) => item.icon !== 'disclaimer-link')
      .map((item) => {
        const parsed = parseEncodedDescription(item.description)
        const ctaLabel = parsed.ctaLabel ?? 'View'
        return {
          id: item.icon ?? item.title,
          eyebrow: parsed.eyebrow,
          title: item.title,
          subtitle: parsed.subtitle,
          image: toImage(item.imageUrl, parsed.imageAlt ?? item.title),
          cta: toLink(ctaLabel, item.url ?? '/'),
        }
      }),
  }
}

function mapPromoTiles(section: CmsPageSection): HomePage['promoTiles'] {
  const disclaimerLinkItem = findItemByIcon(section, 'disclaimer-link')

  return {
    tiles: section.items
      .filter((item) => item.icon !== 'disclaimer-link')
      .map((item) => {
        const parsed = parseEncodedDescription(item.description)
        const linkLabel = parsed.ctaLabel ?? 'Shop now'
        return {
          id: item.icon ?? item.title,
          title: item.title,
          subtitle: parsed.subtitle,
          image: toImage(item.imageUrl, parsed.imageAlt ?? item.title),
          link: toLink(linkLabel, item.url ?? '/'),
        }
      }),
    disclaimer: section.description ?? undefined,
    disclaimerLink: disclaimerLinkItem
      ? toLink(disclaimerLinkItem.title, disclaimerLinkItem.url ?? section.url)
      : section.url
        ? toLink('Details', section.url)
        : undefined,
  }
}

function mapCategoryNav(section: CmsPageSection): HomePage['categoryNav'] {
  return {
    items: section.items.map((item) => toLink(item.title, item.url ?? '/')),
  }
}

function mapFeaturedShop(section: CmsPageSection): HomePage['featuredShop'] {
  return {
    sections: section.items.map((item) => {
      const parsed = parseEncodedDescription(item.description)
      const linkLabel = parsed.ctaLabel ?? 'Shop now'
      return {
        id: item.icon ?? item.title,
        title: item.title,
        subtitle: parsed.subtitle,
        image: toImage(item.imageUrl, parsed.imageAlt ?? item.title),
        link: toLink(linkLabel, item.url ?? '/'),
      }
    }),
  }
}

function mapFooterColumns(sections: CmsPageSection[]): HomePage['footer']['columns'] {
  return sections.map((section) => ({
    title: section.title,
    links: section.items.map((item) => toLink(item.title, item.url ?? '/')),
  }))
}

function mapSiteFooter(section: CmsPageSection, columns: CmsPageSection[]): HomePage['footer'] {
  const copyrightItem = findItemByIcon(section, 'copyright')
  const legalLinks = section.items
    .filter((item) => item.icon === 'legal')
    .map((item) => toLink(item.title, item.url ?? '/'))

  return {
    columns: mapFooterColumns(columns),
    newsletterTitle: section.title,
    newsletterPlaceholder: section.description ?? '',
    newsletterButton: section.url,
    copyright: copyrightItem?.title ?? '',
    legalLinks,
  }
}

export function mapCmsPageToHomePage(
  page: CmsPage,
  options: { includeHomeContent?: boolean } = {},
): HomePage {
  const includeHomeContent = options.includeHomeContent ?? true
  const base = emptyHomePage()

  const footerColumns: CmsPageSection[] = []
  let siteFooterSection: CmsPageSection | null = null

  for (const section of page.sections) {
    const typeName = resolveSectionTypeKey(section.sectionTypeName)

    if (!includeHomeContent && CMS_HOME_SECTION_TYPES.has(typeName)) {
      continue
    }

    if (!includeHomeContent && !CMS_CHROME_SECTION_TYPES.has(typeName)) {
      continue
    }

    switch (typeName) {
      case CMS_SECTION_TYPES.promoAnnouncement:
        base.promoAnnouncement = mapPromoAnnouncement(section)
        break
      case CMS_SECTION_TYPES.utilityBar:
        base.utilityBar = mapUtilityBar(section)
        break
      case CMS_SECTION_TYPES.siteHeader:
        base.header = { ...base.header, ...mapSiteHeader(section) }
        break
      case CMS_SECTION_TYPES.designServicesStrip:
        base.designServices = mapDesignServices(section)
        break
      case CMS_SECTION_TYPES.heroCarousel:
        base.hero = mapHero(section)
        break
      case CMS_SECTION_TYPES.promoTileStrip:
        base.promoTiles = mapPromoTiles(section)
        break
      case CMS_SECTION_TYPES.categoryNav:
        base.categoryNav = mapCategoryNav(section)
        break
      case CMS_SECTION_TYPES.featuredShopGrid:
        base.featuredShop = mapFeaturedShop(section)
        break
      case CMS_SECTION_TYPES.footerColumn:
        footerColumns.push(section)
        break
      case CMS_SECTION_TYPES.siteFooter:
        siteFooterSection = section
        break
      default:
        break
    }
  }

  if (siteFooterSection) {
    base.footer = mapSiteFooter(siteFooterSection, footerColumns)
  } else if (footerColumns.length > 0) {
    base.footer = {
      ...base.footer,
      columns: mapFooterColumns(footerColumns),
    }
  }

  return base
}
