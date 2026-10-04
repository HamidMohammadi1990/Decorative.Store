import i18n from 'i18next'
import { initReactI18next } from 'react-i18next'
import fa from '@/i18n/locales/fa.json'
import en from '@/i18n/locales/en.json'
import type { Locale } from '@/models/shared/locale.model'

void i18n.use(initReactI18next).init({
  resources: {
    en: { translation: en },
    fa: { translation: fa },
  },
  lng: 'en',
  fallbackLng: 'en',
  interpolation: { escapeValue: false },
})

export function syncI18nLocale(locale: Locale): void {
  void i18n.changeLanguage(locale)
}

export default i18n
