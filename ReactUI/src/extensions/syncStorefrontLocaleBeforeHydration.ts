import { syncI18nLocale } from '@/i18n'
import type { Locale } from '@/models/shared/locale.model'
import { useSettingsStore } from '@/stores/settingsStore'

/** Align zustand locale with SSR before React hydration to avoid refetching loader data. */
export async function syncStorefrontLocaleBeforeHydration(ssrLocale?: Locale) {
  await new Promise<void>((resolve) => {
    const applyLocale = () => {
      if (ssrLocale) {
        useSettingsStore.setState({ locale: ssrLocale })
        syncI18nLocale(ssrLocale)
      } else {
        syncI18nLocale(useSettingsStore.getState().locale)
      }
      resolve()
    }

    if (useSettingsStore.persist.hasHydrated()) {
      applyLocale()
      return
    }

    useSettingsStore.persist.onFinishHydration(applyLocale)
  })
}
