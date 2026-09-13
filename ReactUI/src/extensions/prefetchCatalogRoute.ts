import type { Locale } from '@/models/shared/locale.model'
import { prefetchCatalogListing } from '@/services/catalogListingCache'
import { useSettingsStore } from '@/stores/settingsStore'

const SKIP_PREFIXES = [
  '/product/',
  '/blog',
  '/cart',
  '/checkout',
  '/account',
  '/search',
  '/compare',
  '/room-layout',
  '/about',
  '/contact',
  '/p/',
] as const

export function isCatalogListingHref(href: string): boolean {
  if (!href.startsWith('/') || href === '/') return false
  return !SKIP_PREFIXES.some((prefix) => href.startsWith(prefix))
}

export function prefetchCatalogRoute(href: string, locale?: Locale): void {
  if (!isCatalogListingHref(href)) return

  const resolvedLocale = locale ?? useSettingsStore.getState().locale
  prefetchCatalogListing(
    {
      pathname: href,
      searchParams: new URLSearchParams(),
    },
    resolvedLocale,
  )
}
