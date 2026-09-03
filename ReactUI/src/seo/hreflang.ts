import { absoluteUrl } from '@/config/site'

export interface HreflangAlternate {
  hreflang: string
  href: string
}

/** hreflang alternates for bilingual storefront (same URL, locale toggled client-side). */
export function buildHreflangAlternates(path: string): HreflangAlternate[] {
  const href = absoluteUrl(path)
  return [
    { hreflang: 'x-default', href },
    { hreflang: 'en', href },
    { hreflang: 'fa', href },
  ]
}
