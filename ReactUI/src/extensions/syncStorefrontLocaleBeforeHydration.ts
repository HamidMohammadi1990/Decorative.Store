import { syncI18nLocale } from '@/i18n'
import type { Locale } from '@/models/shared/locale.model'
import { useSettingsStore } from '@/stores/settingsStore'

/** Sync i18n to SSR locale before hydration — user settings locale is preserved. */
export async function syncStorefrontLocaleBeforeHydration(ssrLocale?: Locale) {
  await new Promise<void>((resolve) => {
    const applyLocale = () => {
      const bootLocale = ssrLocale ?? useSettingsStore.getState().locale
      syncI18nLocale(bootLocale)
      resolve()
    }

    if (useSettingsStore.persist.hasHydrated()) {
      applyLocale()
      return
    }

    useSettingsStore.persist.onFinishHydration(applyLocale)
  })
}
