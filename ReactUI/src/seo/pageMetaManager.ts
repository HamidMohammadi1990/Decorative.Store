import { SITE_NAME, TWITTER_HANDLE } from '@/config/site'
import { OG_IMAGE_HEIGHT, OG_IMAGE_WIDTH, OG_LOCALE_MAP } from '@/seo/ogConstants'

const MANAGED_SELECTOR = '[data-seo-managed]'

export type PageMetaType = 'website' | 'article' | 'product'

export interface PageMetaInput {
  title: string
  description?: string
  canonical?: string
  image?: string
  imageAlt?: string
  imageWidth?: number
  imageHeight?: number
  type?: PageMetaType
  locale?: string
  noindex?: boolean
  jsonLd?: object | object[] | null
  alternates?: { hreflang: string; href: string }[]
  keywords?: string
  author?: string
  publishedTime?: string
  modifiedTime?: string
  productPrice?: number
  productCurrency?: string
}

let defaultTitle = ''
let defaultDescription = ''

export function setPageMetaDefaults(title: string, description: string) {
  defaultTitle = title
  defaultDescription = description
}

function upsertMeta(
  key: string,
  content: string,
  kind: 'name' | 'property' = 'name',
) {
  if (!content) {
    document.querySelector(`meta[${kind}="${key}"][${MANAGED_SELECTOR.slice(1, -1)}]`)?.remove()
    return
  }

  let el = document.querySelector<HTMLMetaElement>(
    `meta[${kind}="${key}"][data-seo-managed]`,
  )
  if (!el) {
    el = document.createElement('meta')
    el.setAttribute(kind, key)
    el.setAttribute('data-seo-managed', '')
    document.head.appendChild(el)
  }
  el.content = content
}

function upsertLink(rel: string, href: string) {
  if (!href) {
    document.querySelector(`link[rel="${rel}"][data-seo-managed]`)?.remove()
    return
  }

  let el = document.querySelector<HTMLLinkElement>(
    `link[rel="${rel}"][data-seo-managed]`,
  )
  if (!el) {
    el = document.createElement('link')
    el.rel = rel
    el.setAttribute('data-seo-managed', '')
    document.head.appendChild(el)
  }
  el.href = href
}

function upsertAlternates(alternates: { hreflang: string; href: string }[] | undefined) {
  document.querySelectorAll('link[rel="alternate"][data-seo-managed]').forEach((node) => {
    node.remove()
  })
  if (!alternates?.length) return

  for (const alternate of alternates) {
    const el = document.createElement('link')
    el.rel = 'alternate'
    el.hreflang = alternate.hreflang
    el.href = alternate.href
    el.setAttribute('data-seo-managed', '')
    document.head.appendChild(el)
  }
}

function upsertJsonLd(data: object | object[] | null | undefined) {
  document.querySelectorAll('script[type="application/ld+json"][data-seo-managed]').forEach((node) => {
    node.remove()
  })
  if (!data) return

  const items = Array.isArray(data) ? data : [data]
  for (const item of items) {
    const script = document.createElement('script')
    script.type = 'application/ld+json'
    script.setAttribute('data-seo-managed', '')
    script.textContent = JSON.stringify(item)
    document.head.appendChild(script)
  }
}

function resolveOgLocale(locale?: string) {
  if (locale === 'fa') return OG_LOCALE_MAP.fa
  if (locale === 'en') return OG_LOCALE_MAP.en
  return undefined
}

function resolveAlternateOgLocale(locale?: string) {
  if (locale === 'fa') return OG_LOCALE_MAP.en
  if (locale === 'en') return OG_LOCALE_MAP.fa
  return undefined
}

function applyDocumentLocale(locale?: string) {
  if (!locale || typeof document === 'undefined') return
  document.documentElement.lang = locale === 'fa' ? 'fa' : 'en'
  document.documentElement.dir = locale === 'fa' ? 'rtl' : 'ltr'
  document.documentElement.dataset.locale = locale
}

function applyOpenGraphImage(meta: PageMetaInput) {
  if (!meta.image) return

  const width = meta.imageWidth ?? OG_IMAGE_WIDTH
  const height = meta.imageHeight ?? OG_IMAGE_HEIGHT

  upsertMeta('og:image', meta.image, 'property')
  upsertMeta('og:image:secure_url', meta.image.startsWith('http://') ? meta.image.replace('http://', 'https://') : meta.image, 'property')
  upsertMeta('og:image:width', String(width), 'property')
  upsertMeta('og:image:height', String(height), 'property')
  if (meta.imageAlt) upsertMeta('og:image:alt', meta.imageAlt, 'property')

  upsertMeta('twitter:image', meta.image)
  if (meta.imageAlt) upsertMeta('twitter:image:alt', meta.imageAlt)
}

function applyTypeSpecificMeta(meta: PageMetaInput) {
  if (meta.type === 'article') {
    if (meta.publishedTime) upsertMeta('article:published_time', meta.publishedTime, 'property')
    if (meta.modifiedTime) upsertMeta('article:modified_time', meta.modifiedTime, 'property')
    if (meta.author) upsertMeta('article:author', meta.author, 'property')
  }

  if (meta.type === 'product') {
    if (meta.productPrice != null) {
      upsertMeta('product:price:amount', String(meta.productPrice), 'property')
    }
    if (meta.productCurrency) {
      upsertMeta('product:price:currency', meta.productCurrency, 'property')
    }
  }
}

export function applyPageMeta(meta: PageMetaInput) {
  if (typeof document === 'undefined') return

  document.title = meta.title
  applyDocumentLocale(meta.locale)

  const description = meta.description ?? defaultDescription
  upsertMeta('description', description)
  if (meta.keywords) upsertMeta('keywords', meta.keywords)
  upsertMeta('robots', meta.noindex ? 'noindex, nofollow' : 'index, follow')

  upsertLink('canonical', meta.canonical ?? '')

  const ogType = meta.type ?? 'website'
  upsertMeta('og:title', meta.title, 'property')
  upsertMeta('og:description', description, 'property')
  upsertMeta('og:type', ogType, 'property')
  upsertMeta('og:site_name', SITE_NAME, 'property')
  if (meta.canonical) upsertMeta('og:url', meta.canonical, 'property')

  const ogLocale = resolveOgLocale(meta.locale)
  const ogLocaleAlternate = resolveAlternateOgLocale(meta.locale)
  if (ogLocale) upsertMeta('og:locale', ogLocale, 'property')
  if (ogLocaleAlternate) upsertMeta('og:locale:alternate', ogLocaleAlternate, 'property')

  applyOpenGraphImage(meta)
  applyTypeSpecificMeta(meta)

  upsertMeta('twitter:card', meta.image ? 'summary_large_image' : 'summary')
  upsertMeta('twitter:site', TWITTER_HANDLE)
  upsertMeta('twitter:creator', TWITTER_HANDLE)
  upsertMeta('twitter:title', meta.title)
  upsertMeta('twitter:description', description)

  upsertAlternates(meta.alternates)
  upsertJsonLd(meta.jsonLd)
}

export function resetPageMeta() {
  if (typeof document === 'undefined') return
  document.title = defaultTitle
  document.querySelectorAll(MANAGED_SELECTOR).forEach((node) => node.remove())
}

function escapeHtml(value: string): string {
  return value
    .replace(/&/g, '&amp;')
    .replace(/"/g, '&quot;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
}

export function resolveHtmlDocumentAttrs(locale?: string): { lang: string; dir: 'ltr' | 'rtl' } {
  const lang = locale === 'fa' ? 'fa' : 'en'
  const dir = locale === 'fa' ? 'rtl' : 'ltr'
  return { lang, dir }
}

/** Sets lang/dir on the root html element for SSR/prerender output. */
export function applyHtmlDocumentAttrsToTemplate(
  template: string,
  locale?: string,
): string {
  const { lang, dir } = resolveHtmlDocumentAttrs(locale)
  return template.replace(/<html\b[^>]*>/i, `<html lang="${lang}" dir="${dir}">`)
}

/** Static HTML head tags for SSR (mirrors applyPageMeta). */
export function serializePageMetaToHtml(meta: PageMetaInput): string {
  const description = meta.description ?? defaultDescription
  const ogType = meta.type ?? 'website'
  const tags: string[] = []

  tags.push(`<title>${escapeHtml(meta.title)}</title>`)
  tags.push(`<meta name="description" content="${escapeHtml(description)}" data-seo-managed />`)

  if (meta.keywords) {
    tags.push(`<meta name="keywords" content="${escapeHtml(meta.keywords)}" data-seo-managed />`)
  }

  const robots = meta.noindex ? 'noindex, nofollow' : 'index, follow'
  tags.push(`<meta name="robots" content="${robots}" data-seo-managed />`)

  if (meta.canonical) {
    tags.push(`<link rel="canonical" href="${escapeHtml(meta.canonical)}" data-seo-managed />`)
  }

  tags.push(`<meta property="og:title" content="${escapeHtml(meta.title)}" data-seo-managed />`)
  tags.push(
    `<meta property="og:description" content="${escapeHtml(description)}" data-seo-managed />`,
  )
  tags.push(`<meta property="og:type" content="${ogType}" data-seo-managed />`)
  tags.push(`<meta property="og:site_name" content="${escapeHtml(SITE_NAME)}" data-seo-managed />`)

  if (meta.canonical) {
    tags.push(`<meta property="og:url" content="${escapeHtml(meta.canonical)}" data-seo-managed />`)
  }

  const ogLocale = resolveOgLocale(meta.locale)
  const ogLocaleAlternate = resolveAlternateOgLocale(meta.locale)
  if (ogLocale) {
    tags.push(`<meta property="og:locale" content="${ogLocale}" data-seo-managed />`)
  }
  if (ogLocaleAlternate) {
    tags.push(`<meta property="og:locale:alternate" content="${ogLocaleAlternate}" data-seo-managed />`)
  }

  if (meta.image) {
    const width = meta.imageWidth ?? OG_IMAGE_WIDTH
    const height = meta.imageHeight ?? OG_IMAGE_HEIGHT
    tags.push(`<meta property="og:image" content="${escapeHtml(meta.image)}" data-seo-managed />`)
    tags.push(
      `<meta property="og:image:secure_url" content="${escapeHtml(meta.image.startsWith('http://') ? meta.image.replace('http://', 'https://') : meta.image)}" data-seo-managed />`,
    )
    tags.push(`<meta property="og:image:width" content="${width}" data-seo-managed />`)
    tags.push(`<meta property="og:image:height" content="${height}" data-seo-managed />`)
    if (meta.imageAlt) {
      tags.push(`<meta property="og:image:alt" content="${escapeHtml(meta.imageAlt)}" data-seo-managed />`)
    }
  }

  if (meta.type === 'article') {
    if (meta.publishedTime) {
      tags.push(`<meta property="article:published_time" content="${escapeHtml(meta.publishedTime)}" data-seo-managed />`)
    }
    if (meta.modifiedTime) {
      tags.push(`<meta property="article:modified_time" content="${escapeHtml(meta.modifiedTime)}" data-seo-managed />`)
    }
    if (meta.author) {
      tags.push(`<meta property="article:author" content="${escapeHtml(meta.author)}" data-seo-managed />`)
    }
  }

  if (meta.type === 'product') {
    if (meta.productPrice != null) {
      tags.push(`<meta property="product:price:amount" content="${meta.productPrice}" data-seo-managed />`)
    }
    if (meta.productCurrency) {
      tags.push(`<meta property="product:price:currency" content="${escapeHtml(meta.productCurrency)}" data-seo-managed />`)
    }
  }

  tags.push(
    `<meta name="twitter:card" content="${meta.image ? 'summary_large_image' : 'summary'}" data-seo-managed />`,
  )
  tags.push(`<meta name="twitter:site" content="${escapeHtml(TWITTER_HANDLE)}" data-seo-managed />`)
  tags.push(`<meta name="twitter:creator" content="${escapeHtml(TWITTER_HANDLE)}" data-seo-managed />`)
  tags.push(`<meta name="twitter:title" content="${escapeHtml(meta.title)}" data-seo-managed />`)
  tags.push(
    `<meta name="twitter:description" content="${escapeHtml(description)}" data-seo-managed />`,
  )
  if (meta.image) {
    tags.push(`<meta name="twitter:image" content="${escapeHtml(meta.image)}" data-seo-managed />`)
    if (meta.imageAlt) {
      tags.push(`<meta name="twitter:image:alt" content="${escapeHtml(meta.imageAlt)}" data-seo-managed />`)
    }
  }

  if (meta.alternates?.length) {
    for (const alternate of meta.alternates) {
      tags.push(
        `<link rel="alternate" hreflang="${escapeHtml(alternate.hreflang)}" href="${escapeHtml(alternate.href)}" data-seo-managed />`,
      )
    }
  }

  if (meta.jsonLd) {
    const items = Array.isArray(meta.jsonLd) ? meta.jsonLd : [meta.jsonLd]
    for (const item of items) {
      tags.push(
        `<script type="application/ld+json" data-seo-managed>${JSON.stringify(item)}</script>`,
      )
    }
  }

  return tags.join('\n    ')
}
