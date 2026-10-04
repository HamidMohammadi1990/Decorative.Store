import { absoluteUrl } from '@/config/site'
import { stripLocaleHintFromPath } from '@/seo/canonicalPath'

export interface HreflangAlternate {
  hreflang: string
  href: string
}

function withLocaleHint(path: string, locale: 'en' | 'fa'): string {
  const cleanPath = stripLocaleHintFromPath(path)
  const [pathname, search = ''] = cleanPath.split('?')
  const params = new URLSearchParams(search)
  params.set('hl', locale)
  const nextSearch = params.toString()
  const nextPath = nextSearch ? `${pathname}?${nextSearch}` : `${pathname}?hl=${locale}`
  return absoluteUrl(nextPath)
}

/** hreflang alternates for bilingual storefront (locale via ?hl= hint + SSR cookie). */
export function buildHreflangAlternates(path: string): HreflangAlternate[] {
  const canonicalPath = stripLocaleHintFromPath(path)
  const defaultHref = absoluteUrl(canonicalPath)

  return [
    { hreflang: 'x-default', href: defaultHref },
    { hreflang: 'en', href: withLocaleHint(path, 'en') },
    { hreflang: 'fa', href: withLocaleHint(path, 'fa') },
  ]
}
