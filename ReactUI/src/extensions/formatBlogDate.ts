import type { Locale } from '@/models/shared/locale.model'

export function formatBlogDate(date: string, locale: Locale) {
  const parsed = new Date(date)
  if (Number.isNaN(parsed.getTime())) return date

  return parsed.toLocaleDateString(locale === 'fa' ? 'fa-IR' : 'en-GB', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
}
