import type { Locale } from '@/models/shared/locale.model'
import { useHydrated } from '@/hooks/useHydrated'
import { useSettingsStore } from '@/stores/settingsStore'

function resolveHydrationBootLocale(routeLoaderLocale?: string): Locale | undefined {
  const ssrBootLocale =
    typeof window !== 'undefined' ? window.__SSR_LOCALE__ : undefined

  if (ssrBootLocale === 'fa' || ssrBootLocale === 'en') return ssrBootLocale
  if (routeLoaderLocale === 'fa' || routeLoaderLocale === 'en') return routeLoaderLocale
  return undefined
}

/**
 * Locale for data fetching and SSR alignment.
 * During hydration boot, matches SSR loader locale; after hydration, uses user settings.
 */
export function useStorefrontLocale(routeLoaderLocale?: string): Locale {
  const settingsLocale = useSettingsStore((s) => s.locale)
  const hydrated = useHydrated()

  if (import.meta.env.SSR) {
    if (routeLoaderLocale === 'fa' || routeLoaderLocale === 'en') {
      return routeLoaderLocale
    }
    return settingsLocale
  }

  if (!hydrated) {
    return resolveHydrationBootLocale(routeLoaderLocale) ?? settingsLocale
  }

  return settingsLocale
}
