import { useEffect } from 'react'
import { syncLocaleFontStylesheet } from '@/extensions/syncLocaleFontStylesheet'
import type { Locale } from '@/models/shared/locale.model'

const STOREFRONT_EN_FONT_ID = 'storefront-zara-fonts'

/** FA: Vazirmatn (same as legacy ReactUI). EN: editorial Google fonts for Zara look. */
export function useStorefrontFonts(locale: Locale) {
  useEffect(() => {
    syncLocaleFontStylesheet(locale)

    const enLink = document.getElementById(STOREFRONT_EN_FONT_ID) as HTMLLinkElement | null
    if (locale === 'fa') {
      enLink?.remove()
      return
    }

    const href =
      'https://fonts.googleapis.com/css2?family=Cormorant+Garamond:ital,wght@0,400;0,500;1,400&family=Libre+Franklin:wght@300;400;500&display=swap'
    const el = enLink ?? document.createElement('link')
    el.id = STOREFRONT_EN_FONT_ID
    el.rel = 'stylesheet'
    el.href = href
    if (!enLink) document.head.appendChild(el)
  }, [locale])
}
