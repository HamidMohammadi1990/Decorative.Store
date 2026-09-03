const MANAGED_SELECTOR = '[data-seo-managed]'

export type PageMetaType = 'website' | 'article' | 'product'

export interface PageMetaInput {
  title: string
  description?: string
  canonical?: string
  image?: string
  type?: PageMetaType
  locale?: string
  noindex?: boolean
  jsonLd?: object | object[] | null
  alternates?: { hreflang: string; href: string }[]
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

export function applyPageMeta(meta: PageMetaInput) {
  if (typeof document === 'undefined') return
  document.title = meta.title

  upsertMeta('description', meta.description ?? defaultDescription)
  upsertMeta('robots', meta.noindex ? 'noindex, nofollow' : 'index, follow')

  upsertLink('canonical', meta.canonical ?? '')

  const ogType = meta.type ?? 'website'
  upsertMeta('og:title', meta.title, 'property')
  upsertMeta('og:description', meta.description ?? defaultDescription, 'property')
  upsertMeta('og:type', ogType, 'property')
  upsertMeta('og:site_name', 'Diba Gallery', 'property')
  if (meta.canonical) upsertMeta('og:url', meta.canonical, 'property')
  if (meta.image) upsertMeta('og:image', meta.image, 'property')
  if (meta.locale) upsertMeta('og:locale', meta.locale === 'fa' ? 'fa_IR' : 'en_US', 'property')

  upsertMeta('twitter:card', meta.image ? 'summary_large_image' : 'summary')
  upsertMeta('twitter:title', meta.title)
  upsertMeta('twitter:description', meta.description ?? defaultDescription)
  if (meta.image) upsertMeta('twitter:image', meta.image)

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

/** Static HTML head tags for SSR (mirrors applyPageMeta). */
export function serializePageMetaToHtml(meta: PageMetaInput): string {
  const description = meta.description ?? defaultDescription
  const ogType = meta.type ?? 'website'
  const tags: string[] = []

  tags.push(`<title>${escapeHtml(meta.title)}</title>`)
  tags.push(`<meta name="description" content="${escapeHtml(description)}" data-seo-managed />`)

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
  tags.push(`<meta property="og:site_name" content="Diba Gallery" data-seo-managed />`)

  if (meta.canonical) {
    tags.push(`<meta property="og:url" content="${escapeHtml(meta.canonical)}" data-seo-managed />`)
  }
  if (meta.image) {
    tags.push(`<meta property="og:image" content="${escapeHtml(meta.image)}" data-seo-managed />`)
  }
  if (meta.locale) {
    const ogLocale = meta.locale === 'fa' ? 'fa_IR' : 'en_US'
    tags.push(`<meta property="og:locale" content="${ogLocale}" data-seo-managed />`)
  }

  tags.push(
    `<meta name="twitter:card" content="${meta.image ? 'summary_large_image' : 'summary'}" data-seo-managed />`,
  )
  tags.push(`<meta name="twitter:title" content="${escapeHtml(meta.title)}" data-seo-managed />`)
  tags.push(
    `<meta name="twitter:description" content="${escapeHtml(description)}" data-seo-managed />`,
  )
  if (meta.image) {
    tags.push(`<meta name="twitter:image" content="${escapeHtml(meta.image)}" data-seo-managed />`)
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
