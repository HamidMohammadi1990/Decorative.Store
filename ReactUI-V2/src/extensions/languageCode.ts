import type { Locale } from '@/models/shared/locale.model'

export function localeFromLanguageCode(code: string): Locale | null {
  const normalized = code.trim().toLowerCase()
  if (normalized === 'fa' || normalized.startsWith('fa-')) return 'fa'
  if (normalized === 'en' || normalized.startsWith('en-')) return 'en'
  return null
}

export function shortLanguageLabel(code: string): string {
  const locale = localeFromLanguageCode(code)
  if (locale === 'fa') return 'FA'
  if (locale === 'en') return 'EN'
  return code.slice(0, 2).toUpperCase()
}

export function localeFromLanguageId(
  languages: { id: number; code: string }[],
  languageId: number,
  fallback: Locale = 'fa',
): Locale {
  const match = languages.find((item) => item.id === languageId)
  if (!match) return fallback
  return localeFromLanguageCode(match.code) ?? fallback
}
