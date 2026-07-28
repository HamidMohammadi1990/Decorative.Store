export const LOCALES = ['en', 'fa'] as const
export type Locale = (typeof LOCALES)[number]

export function isLocale(value: string): value is Locale {
  return LOCALES.includes(value as Locale)
}
