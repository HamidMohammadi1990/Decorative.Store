import type { Locale } from '@/models/shared/locale.model'
import { useSettingsStore } from '@/stores/settingsStore'

/** Locale aligned with SSR loader data during server render and hydration boot. */
export function useStorefrontLocale(routeLoaderLocale?: string): Locale {
  const settingsLocale = useSettingsStore((s) => s.locale)
  const ssrBootLocale =
    typeof window !== 'undefined' ? window.__SSR_LOCALE__ : undefined

  if (import.meta.env.SSR) {
    if (routeLoaderLocale === 'fa' || routeLoaderLocale === 'en') {
      return routeLoaderLocale
    }
    return settingsLocale
  }

  if (ssrBootLocale && settingsLocale === ssrBootLocale) {
    return ssrBootLocale
  }

  return settingsLocale
}
