import { useEffect } from 'react'
import { useTranslation } from 'react-i18next'
import { getDirection } from '@/extensions/getDirection'
import { syncI18nLocale } from '@/i18n'
import type { Locale } from '@/models/shared/locale.model'
import { useSettingsStore } from '@/stores/settingsStore'

export function useLocaleSettings() {
  const { t } = useTranslation()
  const locale = useSettingsStore((s) => s.locale)
  const currency = useSettingsStore((s) => s.currency)
  const setLocale = useSettingsStore((s) => s.setLocale)
  const hydrateCurrency = useSettingsStore((s) => s.hydrateCurrency)

  useEffect(() => {
    void hydrateCurrency()
  }, [hydrateCurrency])

  useEffect(() => {
    syncI18nLocale(locale)
    const dir = getDirection(locale)
    document.documentElement.lang = locale
    document.documentElement.dir = dir
  }, [locale])

  const switchLocale = async (next: Locale) => {
    await setLocale(next)
  }

  return {
    locale,
    currency,
    direction: getDirection(locale),
    switchLocale,
    t,
  }
}
