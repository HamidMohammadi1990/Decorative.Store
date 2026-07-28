import type { Locale } from '@/models/shared/locale.model'

export function getDirection(locale: Locale): 'ltr' | 'rtl' {
  return locale === 'fa' ? 'rtl' : 'ltr'
}
