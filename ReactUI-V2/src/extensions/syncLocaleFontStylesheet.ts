import type { Locale } from '@/models/shared/locale.model'

const FONT_LINK_ID = 'locale-font-stylesheet'

const FONT_URLS: Record<Locale, string> = {
  en: 'https://fonts.googleapis.com/css2?family=DM+Sans:ital,opsz,wght@0,9..40,400;0,9..40,500;0,9..40,600;0,9..40,700;1,9..40,400&display=swap',
  fa: 'https://cdn.jsdelivr.net/gh/rastikerdar/vazirmatn@v33.003/Vazirmatn-font-face.css',
}

/** Loads only the font needed for the active locale (reduces unused CSS). */
export function syncLocaleFontStylesheet(locale: Locale) {
  if (typeof document === 'undefined') return

  let link = document.getElementById(FONT_LINK_ID) as HTMLLinkElement | null
  const href = FONT_URLS[locale]

  if (!link) {
    link = document.createElement('link')
    link.id = FONT_LINK_ID
    link.rel = 'stylesheet'
    link.href = href
    document.head.appendChild(link)
    return
  }

  if (link.href !== href) {
    link.href = href
  }
}

export function readInitialLocale(): Locale {
  try {
    const raw = localStorage.getItem('westelm-settings')
    const parsed = raw ? JSON.parse(raw) : null
    const locale = parsed?.state?.locale
    return locale === 'fa' ? 'fa' : 'en'
  } catch {
    return 'en'
  }
}
